using System.Collections.Generic;
using UnityEngine;

namespace Character
{
    [CreateAssetMenu(fileName = "MartialAction", menuName = "Characters/Actions/Job", order = 1)]
    public class MartialAction : Action
    {
        protected override void Execute(SortedSet<int> targets)
        {
            throw new System.NotImplementedException();
        }
    }
}