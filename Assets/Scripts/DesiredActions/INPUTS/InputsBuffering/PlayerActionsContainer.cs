using System.Collections.Generic;
using System.Linq;
public class PlayerActionsContainer
{
    private List<PlayerAction> m_ListOfActions;

    public PlayerActionsContainer()
    {
        this.m_ListOfActions = new List<PlayerAction>();
    }

    public void AddAction(PlayerAction _newAction)
    {
        this.m_ListOfActions.Add(_newAction);
    }

    public bool Contains(PlayerActionsType _actionType)
    {
        return m_ListOfActions.Exists(action => action.actionType == _actionType);
    }

    public void ConsumeAllActions(PlayerActionsType _actionType)
    {
        this.m_ListOfActions.RemoveAll(action => action.actionType == _actionType);
    }

    public void OnUpdateActions()
    {
        foreach(PlayerAction action in m_ListOfActions)
        {
            action.GetOlder();
        }
        this.m_ListOfActions.RemoveAll(action => action.IsTooOld());
    }

    public void PurgeAllAction()
    {
        this.m_ListOfActions.Clear();
    }
}
