namespace Cobalt.Pipeline.Rules.Sample
{
    public class MySet : CobaltRuleSet<MyObj>
    {
        public CobaltRule<MyObj> StartsWithA =>
            Specify()
                .When(x => x.StringProp.StartsWith("a"))
                .Then("String started with a")
                .Else("String did not start with a")
                .Build();


        public CobaltRule<MyObj> Numis1 =>
            Specify()
                .When(x => x.IntProp == 1)
                .Then("int is 1")
                .Else("int is not 1")
                .Build();

        public CobaltRule<MyObj> StringIsShort =>
            Specify()
                .When(x => x.StringProp.Length > 50)
                .NotSatisfied().Then("String is longer then 50 so it is not short")
                .Else("string is shorter than 50")
                .Build();
    }
}