﻿using System.Dynamic;
using System.Threading.Tasks;
using Cobalt;
using Cobalt.Channel.Delimited.Input;
using Cobalt.Pipeline.Channel.Local;
using Cobalt.Pipeline.Operation;
using Cobalt.Unit;

namespace Cobalt64
{
    internal static class Program
    {
        private static async Task Main(string[] args)
        {
            // CobaltUnits are compatable with EnxpandoObjects and dynamics 
            dynamic first = new CobaltUnit();
            first.PropertyA = "Alphabet";
            first.PropertyB = "Baseball";

            dynamic second = new CobaltUnit();
            second.PropertyA = "Apple";
            second.PropertyB = "Dog";
            second.PropertyC = "Cat";
            
            
            InMemInputChannel inputChannel = new InMemInputChannel();
            inputChannel.AddUnit(first);
            inputChannel.AddUnit(second);
            
            
            var pipe = Cblt.Pipeline
                .Channel(inputChannel)
                .Stage(builder =>
                        builder
                            .Op(new FilterStep(unit => unit.PropertyA.ToString().StartsWith("A")))
                            .Op(new FilterStep(unit => unit.PropertyB.ToString().StartsWith("B"))));
            
            await pipe.ExecuteAsync();
            
            
        }
    }
}