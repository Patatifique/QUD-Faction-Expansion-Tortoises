using System;
using System.Collections.Generic;
using Wintellect.PowerCollections;
using XRL.Collections;
using XRL.Rules;
using XRL.World.Effects;
using XRL.World.Parts;


// This is essentially an exact copy of GoOnAPilgrimage, with slight changes to refer to AiPilgrimGraveyard instead of AiPilgrim as a dirty fix
#nullable disable
namespace XRL.World.AI.GoalHandlers
{


    [Serializable]
    public class GoOnAGraveyardPilgrimage : GoalHandler
    {
        public static int TargetWx = 5;
        public static int TargetWy = 2;
        public static int TargetXx = 1;
        public static int TargetYx = 1;
        public static int TargetZx = 10;
        public string TargetObject = "StiltWell";
        public string TargetZoneID = "JoppaWorld.5.2.1.1.10";
        public string TargetEntranceZoneID = "JoppaWorld.5.2.1.2.10";

        public GoOnAGraveyardPilgrimage()
        {
        }

        public GoOnAGraveyardPilgrimage(
          int Wx,
          int Wy,
          int Xx,
          int Yx,
          int Zx,
          string targetObject,
          string zoneID,
          string entranceZoneID)
        {
            GoOnAGraveyardPilgrimage.TargetWx = Wx;
            GoOnAGraveyardPilgrimage.TargetWy = Wy;
            GoOnAGraveyardPilgrimage.TargetXx = Xx;
            GoOnAGraveyardPilgrimage.TargetYx = Yx;
            GoOnAGraveyardPilgrimage.TargetZx = Zx;
            this.TargetObject = targetObject;
            this.TargetZoneID = zoneID;
            this.TargetEntranceZoneID = entranceZoneID;
        }

        public override bool Finished() => false;

        public override void Create()
        {
        }

        public void MoveToTargetZone()
        {
            if (this.CurrentZone.ZoneID == this.TargetEntranceZoneID)
            {
                if (this.CurrentCell.X == 38 && this.CurrentCell.Y == 0)
                    this.ParentBrain.PushGoal((GoalHandler)new Step("N"));
                else
                    this.ParentBrain.PushGoal((GoalHandler)new MoveTo(this.TargetEntranceZoneID, 37 + Stat.Random(0, 3), 0));
            }
            else
                this.ParentBrain.PushGoal((GoalHandler)new MoveToGlobal(this.TargetEntranceZoneID, 37 + Stat.Random(0, 3), 0));
        }

        private bool HasAdjacentObject(string Blueprint)
        {
            foreach (Cell localAdjacentCell in this.ParentObject.CurrentCell.GetLocalAdjacentCells())
            {
                foreach (GameObject gameObject in (Container<GameObject>)localAdjacentCell.Objects)
                {
                    if (gameObject.Blueprint == Blueprint)
                        return true;
                }
            }
            return false;
        }

        public void MoveToWell()
        {
            if (this.TargetObject == null)
                this.TargetObject = this.CurrentZone.GetObjectsWithPart("Brain").GetRandomElement<GameObject>().GetBlueprint().Name;
            if (this.HasAdjacentObject(this.TargetObject))
            {
                this.ParentObject.GetPart<AiPilgrimGraveyard>().FoundTarget = true;
                this.Pop();
                this.ParentBrain.PushGoal((GoalHandler)new WanderRandomly(6));
            }
            else
            {
                GameObject objectExcludingSelf = this.CurrentZone.FindObjectExcludingSelf(this.TargetObject, this.ParentObject);
                if (objectExcludingSelf != null)
                {
                    List<Cell> localAdjacentCells = objectExcludingSelf.Physics.CurrentCell.GetLocalAdjacentCells();
                    Algorithms.RandomShuffleInPlace<Cell>((IList<Cell>)localAdjacentCells, Stat.Rand);
                    for (int index = 0; index < localAdjacentCells.Count; ++index)
                    {
                        if (localAdjacentCells[index].IsEmpty())
                        {
                            this.ParentBrain.PushGoal((GoalHandler)new MoveTo(this.CurrentZone.ZoneID, localAdjacentCells[index].X, localAdjacentCells[index].Y));
                            return;
                        }
                    }
                    this.ParentBrain.PushGoal((GoalHandler)new WanderRandomly(6));
                }
                else
                    this.FailToParent();
            }
        }

        public override void TakeAction()
        {
            if (this.ParentObject.HasEffect<Lost>() || this.ParentObject.HasEffect<XRL.World.Effects.Confused>() && !this.ParentObject.HasEffect<FuriouslyConfused>())
                this.FailToParent();
            else if (this.ParentObject.InZone(this.TargetZoneID))
                this.MoveToWell();
            else
                this.MoveToTargetZone();
        }
    }
}
