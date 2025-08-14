using UnityEngine;

public class EndingHUDPresenter
{
    private EndingHUDView view;
    private YSJ_GameManager modelGM;

    public EndingHUDPresenter(EndingHUDView view, YSJ_GameManager modelGM)
    {
        this.view = view;
        this.modelGM = modelGM;

        this.view.UpdateTotalScore(this.modelGM.Score);
        this.view.UpdateEndingState(this.modelGM.endCatSO, this.modelGM.Score);
        this.view.Open();
    }
}
