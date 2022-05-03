There are three primary components to a plant:

PlantInfo (Such as in Data/Plants/Ranunculus/RanunculusInfo):
	This is the base data object for a single plant. It holds a number of sequential plant stages in an array, as well as a string name for the appropriate minigame scene).
	
PlantStageInfo (Such as in Data/Plants/Ranunculus/Seed/Seed):
	This holds the data for a single plant stage. Multiple of these are inserted into a single PlantInfo.
	This object can also hold any number of HelpSteps.
	
HelpStep (Such as in Data/Plants/Ranunculus/Seed/HelpStep1):
	These objects hold the hints and variable lock states.
	The 'next' HelpStep's hint is shown when the button is pressed. (ie. on just starting, the first HelpStep's hint is shown. After a single failure the second HelpStep's hint is shown).
	Locked variables are cumulative. if 2 failures are accumulated, both the first and second HelpStep's locks will be applied.
	

Requirements:
	All enums have several gameplay values (eg. WaterLevel's sparce, regular, and heavy) as well as 2 extra functional values:
		Disabled: Removes the interactable from the scene and ignores variable when checking conditions
		Uninteractable: Keeps the interactable in the scene visually but disables controls. Also ignores variable on checking conditions.
		

Example:
This is how a plant is structured in data

PlantInfo
	Stage (Seed)
		HelpStep (lock depth, hint about depth)
		HelpStep (lock soil type, hint about soil type)
		HelpStep (lock water level, hint about water level)
	Stage (Seedling)
		HelpStep (lock water level, hint about water level)
		HelpStep (lock sun exposure, hint about sun exposure)
		HelpStep (lock fertiliser, hint about fertiliser)
	Stage (Flowering)
		HelpStep (lock pollinator, hint about pollinator)
	Stage (Fruiting)
		HelpStep (lock dispersal, hint about dispersal)