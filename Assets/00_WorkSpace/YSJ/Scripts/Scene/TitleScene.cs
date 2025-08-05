using UnityEngine;

namespace Scripts.Scene
{
    public class TitleScene : SceneBase
    {
        public override SceneID SceneID => SceneID.TitleScene;

        [Header("UI View")]
        [SerializeField] private TitleHUDView titleHUDView;


        protected override void Initialize()
        {

        }
    }
}
