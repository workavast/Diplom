using App.UI;
using UnityEngine;
using Zenject;

namespace App.Session.Creation
{
    public class SessionCreatorInstaller : MonoInstaller
    {
        [SerializeField] private WaitScreen waitScreen;
        
        public override void InstallBindings()
        {
            // waitScreen = waitScreen.IsNullFindOfType();
            
            Container.BindInterfacesAndSelfTo<SessionCreator>().FromNew().AsSingle().WithArguments(waitScreen);
        }
    }
}