using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using AdaptiveCards;
using AdaptiveCards.Rendering.Wpf;
using System.Diagnostics;

namespace metering.cards
{
    class CommunicationCard
    {
        AdaptiveCardRenderer renderer;
        AdaptiveCard communicationCard;

        public void RenderCommunicationCard()
        {
            // create a card renderer.
            renderer = new AdaptiveCardRenderer();

            // Using the Xceed package, so enable the enhanced input.
            renderer.UseXceedElementRenderers();

            // Debugging check: Verify the schema version this renderer supports.
            AdaptiveSchemaVersion schemaVersion = renderer.SupportedSchemaVersion;

            // Create communication card.
            CreateCommunicationCard();

            // Render the card and enable the actions.
            RenderCommunicationCard(communicationCard);
        }

        private void CreateCommunicationCard()
        {
            // Create the card, and provide required Id.
            communicationCard = new AdaptiveCard("1.0")
            {
                Id = "Communication Card"
            };

            // Add the title text to the card and set a few attributes.
            communicationCard.Body.Add(new AdaptiveTextBlock
            {
                Text = "First Cards line",
                Size = AdaptiveTextSize.ExtraLarge,
                IsSubtle = true
            });

            // Add a description of the card, in a smaller font.
            communicationCard.Body.Add(new AdaptiveTextBlock
            {
                Text = "Second Cards line",
                Size = AdaptiveTextSize.Medium,
                IsSubtle = true
            });
        }

        private void RenderCommunicationCard(AdaptiveCard communicationCard)
        {
            try
            {
                // Render the card.
                RenderedAdaptiveCard renderedCard = renderer.RenderCard(communicationCard);

                // Get the FrameworkElement and attach it to our Frame element in the UI.


                // Debugging check: Look for any renderer warnings.
                // This includes things like an unknown element type found in the card
                // or the card exceeded the maximum number of supported actions, and so on.
                IList<AdaptiveWarning> warnings = renderedCard.Warnings;

                for (int i = 0; i < warnings.Count; i++)
                {
                    Console.WriteLine(warnings[i].Message);
                }

            }
            catch (AdaptiveException ex)
            {
                // Display error.
                Debug.WriteLine(ex.Message);
                throw;
            }
        }
    }
}
