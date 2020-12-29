using System;
using System.Collections.Generic;

namespace Cobalt
{
    public class CoPipe
    {
     
        public List<CoStage> Stages { get; set; }


        public CoPipe()
        {
            Stages = new List<CoStage>();
        }


        

   
        
        public CoPipe In(params CoUnit[] units)
        {
            var newUnits = new List<CoUnit>();
            foreach (var unit in units)
            {
                foreach (var stage in Stages) 
                    stage.PrepareInteractionPlan(unit);

                newUnits.Add(unit);
            }
          
            return this;
        }
        

        public CoPipe Stage<TStage>(Action<TStage> stageConfig) where TStage: CoStage
        {
            CoStage coStage = Activator.CreateInstance<TStage>();
            stageConfig((TStage)coStage);
            Stages.Add(coStage);
            return this;
        }
    }
}