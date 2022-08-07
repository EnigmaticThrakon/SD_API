# ViewModels

These are models that are portions of the actual model and are used for the API endpoints. The purpose of this is so that the external device sending data to the endpoint doesn't need to consider all the information being used by the server.

**The models that are contained within the other repositories that will be gathering/altering information on this server must match the corresponding view model in this project.***

***Note:***

The view models are only used for organizing data coming in through the API or being sent out by one, if the data is to be passed further into the server then the data must be translated into a base model.