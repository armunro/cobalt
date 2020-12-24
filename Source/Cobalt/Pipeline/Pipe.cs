using System;
using System.Collections.Generic;

namespace Cobalt.Pipeline
{
    public class Pipe
    {
     
        public List<Stage.Stage> Stages { get; set; }


        public Pipe()
        {
            Stages = new List<Stage.Stage>();
        }


        

   
        
        public Pipe In(params Unit.Unit[] units)
        {
            var newUnits = new List<Unit.Unit>();
            foreach (var unit in units)
            {
                foreach (var stage in Stages) 
                    stage.PrepareInteractionPlan(unit);

                newUnits.Add(unit);
            }
          
            return this;
        }
        

        public Pipe Stage<TStage>(Action<TStage> stageConfig) where TStage: Stage.Stage
        {
            Stage.Stage stage = Activator.CreateInstance<TStage>();
            stageConfig((TStage)stage);
            Stages.Add(stage);
            return this;
        }
    }
}