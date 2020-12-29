using System;
using System.Collections.Generic;

namespace Cobalt.Core
{
    public class Pipe
    {
     
        public List<Stage> Stages { get; set; }


        public Pipe()
        {
            Stages = new List<Stage>();
        }


        

   
        
        public Pipe In(params Unit[] units)
        {
            var newUnits = new List<Unit>();
            foreach (var unit in units)
            {
                foreach (var stage in Stages) 
                    stage.PrepareInteractionPlan(unit);

                newUnits.Add(unit);
            }
          
            return this;
        }
        

        public Pipe Stage<TStage>(Action<TStage> stageConfig) where TStage: Stage
        {
            Stage stage = Activator.CreateInstance<TStage>();
            stageConfig((TStage)stage);
            Stages.Add(stage);
            return this;
        }
    }
}