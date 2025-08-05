using UnityEngine;

namespace Scripts.Scene
{
    public class InGameScene : SceneBase
    {
        public override SceneID SceneID => SceneID.InGameScene;

        [SerializeField] private InGameHUDView _inGameHUDView;
        private InGameHUDPresenter _inGameHUDPresenter;

        protected override void Initialize()
        {
            base.Initialize();
            _inGameHUDPresenter = new(_inGameHUDView, YSJ_GameManager.Instance);
        }
    }
}
