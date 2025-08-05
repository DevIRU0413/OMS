public class YSJ_PanelBaseUI : YSJ_BaseUI
{
    #region Field
    public override YSJ_UIType UIType => YSJ_UIType.Panel;

    #endregion

    #region Unity Method
    private void Start() => Open();

    #endregion
}