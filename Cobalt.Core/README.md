#Cobalt
Clean automation pipelines for everyone and everything.

##Work 


Cobalt work are objects that fully describe how a CobaltObject should be interacted with as it passes through the pipeline stage. Works should seek to shape data as little as possible, keeping the size of these work small, effecient and easily distributable.

###WorkTypes
Several different types of works exist to acomplish a variety of shaping tasks. They are as follows:

- StructuralWork: Alters the shape/formatting of the Cobalt Data 
- FactualWork: Alters the values (facts) in the persistant data structure
- LogicalWork: Rules define standardized decisions that can made manually, automatically or both.
- IterativeWork: Loop over collections and feed nested/linked pipelines  
- ExecutableWork: Do something completely different like print a page. Either a human or a computer can do this. This may start off as a manual task for a human, but become an automated task as development of techology allows.

###WorkPlans
Work plans can be described as the 'checklists' that dictate the how a stage should behave. WorkPlans consist of:
- Sequential _or_ heirarchical list of works
- Failure pipelines (paths to take if an work or stage fails)
- Possible failure mitigation
- Failure detection -> alerting -> remediation -> repair

