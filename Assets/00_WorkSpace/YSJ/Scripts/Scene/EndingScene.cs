using UnityEngine;

namespace Scripts.Scene
{
    public class EndingScene : SceneBase
    {
        public override SceneID SceneID => SceneID.EndingScene;

        [SerializeField] private EndingHUDView _endingHUDView;

        private EndingHUDPresenter _endingHUDPresenter;

        protected override void Initialize()
        {
            base.Initialize();
            if (_endingHUDView != null)
            {
                _endingHUDView.InitBaseUI();
                _endingHUDPresenter = new(_endingHUDView, YSJ_GameManager.Instance);
            }
        }
    }
}
