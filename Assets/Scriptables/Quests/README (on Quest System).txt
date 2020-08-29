VERY IMPORTANT NOTE!!!

<On Structure of Quest System>
- "Subject" refers to the "do-er" of a QuestEvent
- "Object" refers to the goal of a QuestEvent

Ex 1)
Subject => Skeleton Warrior 
QuestEvent => Death
Object => Empty

the above means "Skeleton warrior's death," not "Skeleton warrior 'kills' nothing"
Therefore, something like 

Subject => Player
QuestEvent => Death
Object => Skeleton Warrior

is not valid. You have interpreted this as "Player killed a Skeleton Warrior," which is erroneous


Ex 2)
Subject => Carriage
QuestEvent => Reached
Object => CaveExitPoint

the above translates to "Carriage entity has reached Cave Exit Point"