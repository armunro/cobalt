using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cobalt.Pipeline.Steps;
using Cobalt.Unit;

namespace Cobalt.Pipeline
{
    public class CobaltPipeline
    {
     
        private List<Steps.Stage> Stages { get; set; }


        public CobaltPipeline()
        {
            Stages = new List<Steps.Stage>();
        }


        

   
        
        public CobaltPipeline In(params CobaltUnit[] units)
        {
            var newUnits = new List<CobaltUnit>();
            foreach (var unit in units)
            {
                foreach (var stage in Stages) 
                    stage.RunStage(unit);

                newUnits.Add(unit);
            }
          
            return this;
        }
        

        public CobaltPipeline Stage<TStage>(Action<TStage> stageConfig) where TStage: Steps.Stage
        {
            Steps.Stage stage = Activator.CreateInstance<TStage>();
            stageConfig((TStage)stage);
            Stages.Add(stage);
            return this;
        }
    }
}