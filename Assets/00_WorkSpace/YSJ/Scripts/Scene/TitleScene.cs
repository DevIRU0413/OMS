using UnityEngine;

namespace Scripts.Scene
{
    public class TitleScene : SceneBase
    {
        public override SceneID SceneID => SceneID.TitleScene;

        [Header("UI View")]
        [SerializeField] private TitleHUDView titleHUDView;
        [SerializeField] private TutorialPanelView tutorialPanelView;

        protected override void Initialize()
        {
            titleHUDView.InitBaseUI();
            tutorialPanelView.InitBaseUI();
        }
    }
}
