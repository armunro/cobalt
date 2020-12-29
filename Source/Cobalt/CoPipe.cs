using System;
using System.Collections.Generic;

namespace Cobalt.Pipeline
{
    public class CoPipe
    {
     
        public List<Stage.CoStage> Stages { get; set; }


        public CoPipe()
        {
            Stages = new List<Stage.CoStage>();
        }


        

   
        
        public CoPipe In(params Unit.CoUnit[] units)
        {
            var newUnits = new List<Unit.CoUnit>();
            foreach (var unit in units)
            {
                foreach (var stage in Stages) 
                    stage.PrepareInteractionPlan(unit);

                newUnits.Add(unit);
            }
          
            return this;
        }
        

        public CoPipe Stage<TStage>(Action<TStage> stageConfig) where TStage: Stage.CoStage
        {
            Stage.CoStage coStage = Activator.CreateInstance<TStage>();
            stageConfig((TStage)coStage);
            Stages.Add(coStage);
            return this;
        }
    }
}