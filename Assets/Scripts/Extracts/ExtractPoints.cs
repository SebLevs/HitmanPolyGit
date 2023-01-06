using UnityEngine;

public class ExtractPoints : Entity_Collider
{
    [SerializeField] private string extractName;
    public float timer;
    private Entity_Player player = Entity_Player.Instance;
    private bool onetime = false;
    [SerializeField] private bool isActive = false;

    public bool IsActive { get => isActive; set => isActive = value; }
    public string ExtractName { get => extractName;}

    protected override void OnStart()
    {
        timer = 5f;
        onetime = false;
        isActive = false;
        GetComponent<CapsuleCollider>().enabled = false;
        GetComponentInChildren<MeshRenderer>().enabled = false;
        ExtractManager.Instance.ExtractPoints.Add(this);
    }

    public void OnActivate()
    {
        GetComponent<CapsuleCollider>().enabled = true;
        GetComponentInChildren<MeshRenderer>().enabled = true;
    }

    public void OnExtraction()
    {
        QuestManager.Instance.currentExtractQuest.point = this;
        QuestManager.Instance.currentExtractQuest.OnCompleteQuest();
        HighScoreManager.Instance.CheckHighScore();
    }

    protected override void OnTriggerEnterAct(Collider collision)
    {
        if (collision.gameObject == player.gameObject)
        {
            timer = 0f;
            UIManager.Instance.View_Extraction.OnShow();
            UIManager.Instance.View_Extraction.ExtractionElement.ResetTitle(true);
            UIManager.Instance.View_highscore.OnShow();
            collision.gameObject.GetComponent<Entity_Player>().canCrouch = false;
        }
    }

    protected override void OnTriggerStayAct(Collider collision)
    {
        if(collision.gameObject == player.gameObject && !player.IsDead)
        {
            timer += Time.deltaTime;
            UIManager.Instance.View_Extraction.ExtractionElement.SetFilling(timer / 5);
            if (timer >= 5f && !onetime)
            {
                timer = 5f;
                onetime = true;
                OnExtraction();

                SceneLoadManager.Instance.OnLoadMainMenu(true);
            }
        }
    }

    protected override void OnTriggerExitAct(Collider collision)
    {
        if (collision.gameObject == player.gameObject)
        {
            UIManager.Instance.View_Extraction.ExtractionElement.SetTitleOutsideExtractionPoint(true);
            UIManager.Instance.View_Extraction.ExtractionElement.ResetFilling();
            UIManager.Instance.View_highscore.OnHide();
            collision.gameObject.GetComponent<Entity_Player>().canCrouch = true;
        }
    }
}
