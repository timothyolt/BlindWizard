using System.Collections.Generic;
using BlindWizard.Data;
using JetBrains.Annotations;

namespace BlindWizard.Interfaces
{
    public interface IActorWorld
    {
        IEnumerable<IActor> Actors { get; }
        WizardLevel this[int level] { get; }
        int Count { get; }
        void AddLevel();
        void AddActor([NotNull] IActor actor);
        void RemoveActor([NotNull] IActor actor);
    }
    
    public delegate void ActionCallback(Action action);
    
    public interface IActor
    {
        int Level { get; }
        RoomId Position { get; }
        void Provide([NotNull] IActorWorld world);
        void GetAction([NotNull] ActionCallback callback);
    }
    
    public struct Action
    {
        public delegate void ActDelegate(IActorWorld world);
        public delegate void FxDelegate(IActorWorld world);

        public Action([NotNull] ActDelegate act, [NotNull] FxDelegate fx)
        {
            Act = act;
            Fx = fx;
        }
        
        [NotNull] public ActDelegate Act { get; }
        [NotNull] public FxDelegate Fx { get; }
    }
}