﻿namespace Microsoft.Bot.Builder.Location.Dialogs
{
    using System;
    using Bing;
    using Builder.Dialogs;
    using Internals.Fibers;
    using Microsoft.Bot.Builder.Location.Azure;

    [Serializable]
    internal class LocationDialogFactory : ILocationDialogFactory
    {
        private readonly string apiKey;
        private readonly string channelId;
        private readonly string prompt;
        private readonly LocationOptions options;
        private readonly LocationRequiredFields requiredFields;
        private readonly IGeoSpatialService geoSpatialService;
        private readonly LocationResourceManager resourceManager;
        private bool useAzureMaps = true;

        internal LocationDialogFactory(
            string apiKey,
            string channelId,
            string prompt,
            IGeoSpatialService geoSpatialService,
            LocationOptions options,
            LocationRequiredFields requiredFields,
            LocationResourceManager resourceManager)
        {
            SetField.NotNull(out this.apiKey, nameof(apiKey), apiKey);
            SetField.NotNull(out this.channelId, nameof(channelId), channelId);
            SetField.NotNull(out this.prompt, nameof(prompt), prompt);
            this.geoSpatialService = geoSpatialService;
            this.options = options;
            this.requiredFields = requiredFields;
            this.resourceManager = resourceManager ?? new LocationResourceManager();

            if(!string.IsNullOrEmpty(this.apiKey) && this.apiKey.Length > 60)
            {
                useAzureMaps = false;
            }
        }

       public IDialog<LocationDialogResponse> CreateDialog(BranchType branch, Location location = null, string locationName = null, bool skipDialogPrompt = false)
        {
            bool isFacebookChannel = StringComparer.OrdinalIgnoreCase.Equals(this.channelId, "facebook");

            if (branch == BranchType.LocationRetriever)
            {
                if (this.options.HasFlag(LocationOptions.UseNativeControl) && isFacebookChannel)
                {
                    return new FacebookNativeLocationRetrieverDialog(
                        this.prompt,
                        this.geoSpatialService,
                        this.options,
                        this.requiredFields,
                        this.resourceManager);
                }

                IGeoSpatialService geoService;

                if (useAzureMaps)
                {
                    geoService = new AzureMapsSpatialService(this.apiKey);
                }
                else
                {
                    geoService = new BingGeoSpatialService(this.apiKey);
                }

                return new RichLocationRetrieverDialog(
                    prompt: this.prompt,
                    supportsKeyboard: isFacebookChannel,
                    cardBuilder: new LocationCardBuilder(this.apiKey, this.resourceManager),
                    geoSpatialService: geoService,
                    options: this.options,
                    requiredFields: this.requiredFields,
                    resourceManager: this.resourceManager,
                    skipPrompt: skipDialogPrompt);
            }
            else if (branch == BranchType.FavoriteLocationRetriever)
            {
                IGeoSpatialService geoService;

                if (useAzureMaps) {
                    geoService = new AzureMapsSpatialService(this.apiKey);
                }
                else {
                    geoService = new BingGeoSpatialService(this.apiKey);
                }

                return new FavoriteLocationRetrieverDialog(
                    isFacebookChannel,
                    new FavoritesManager(),
                    this,
                    new LocationCardBuilder(this.apiKey, this.resourceManager),
                    geoService,
                    this.options,
                    this.requiredFields,
                    this.resourceManager);
            }
            else if (branch == BranchType.AddToFavorites)
            {
                return new AddFavoriteLocationDialog(new FavoritesManager(), location, this.resourceManager);
            }
            else if (branch == BranchType.EditFavoriteLocation)
            {
                return new EditFavoriteLocationDialog(this, new FavoritesManager(), locationName, location, this.resourceManager);
            }
            else
            {
                throw new ArgumentException("Invalid branch value.");
            }
        }
    }
}
