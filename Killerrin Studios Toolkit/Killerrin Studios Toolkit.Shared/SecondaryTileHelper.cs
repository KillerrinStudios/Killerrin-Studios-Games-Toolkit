using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Popups;
using Windows.UI.StartScreen;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using System.Diagnostics;

namespace KillerrinStudiosToolkit
{
    public static class SecondaryTileHelper
    {
        public static Rect GetElementRect(FrameworkElement element)
        {
            GeneralTransform buttonTransform = element.TransformToVisual(null);
            Point point = buttonTransform.TransformPoint(new Point());
            return new Rect(point, new Size(element.ActualWidth, element.ActualHeight));
        }

        #region Helper Methods
        #endregion

        #region Secondary Tile Methods
        public static bool DoesTileExist(string tileID)
        {
            try {
                if (Windows.UI.StartScreen.SecondaryTile.Exists(tileID)) {
                    return true;
                }
                else {
                    return false;
                }
            }
            catch (Exception) { return false; }
        }


#if WINDOWS_APP
        public static async Task<bool> CreateTile(object sender, string tileID, string textOnTile, string activationArguments, Uri _tileImage, TileSize tileSize)
#elif WINDOWS_PHONE_APP
        public static async Task<bool> CreateTile(string tileID, string textOnTile, string activationArguments, Uri _tileImage, TileSize tileSize)
#endif
        {
            Debug.WriteLine("CreateTile(): Entering");
            //-- Prepare package images for all four tile sizes in our tile to be pinned as well as for the square30x30 logo used in the Apps view.  

            Uri tileImage;
            if (_tileImage.OriginalString.Contains("http://")) {
                if (!await StorageTools.DoesFolderExist(StorageTools.StorageConsts.TileFolder)) {
                    await StorageTools.CreateFolder(StorageTools.StorageConsts.TileFolder);
                }

                await StorageTools.SaveFileFromServer(_tileImage, StorageTools.StorageConsts.TileFolder + "\\" + tileID + ".jpg");
                tileImage = new Uri(StorageTools.StorageConsts.LocalStorageFolderPrefix +
                                            StorageTools.StorageConsts.TileFolder + "\\" +
                                            tileID +
                                            ".jpg",
                                        UriKind.Absolute);
            }
            else {
                tileImage = _tileImage;
            }


            //-- During creation of secondary tile, an application may set additional arguments on the tile that will be passed in during activation.
            //-- These arguments should be meaningful to the application. In this sample, we'll pass in the date and time the secondary tile was pinned.

            // Create a Secondary tile with all the required arguments.
            // Note the last argument specifies what size the Secondary tile should show up as by default in the Pin to start fly out.
            // It can be set to TileSize.Square150x150, TileSize.Wide310x150, or TileSize.Default.  
            // If set to TileSize.Wide310x150, then the asset for the wide size must be supplied as well.
            // TileSize.Default will default to the wide size if a wide size is provided, and to the medium size otherwise. 
            Debug.WriteLine("Making Tile");
            SecondaryTile secondaryTile = new SecondaryTile(tileID,
                                                            textOnTile,
                                                            activationArguments,
                                                            tileImage,
                                                            tileSize);

#if WINDOWS_APP
            Debug.WriteLine("Setting Wide310x150Logo && Square310x310Logo");
            // Only support of the small and medium tile sizes is mandatory. 
            // To have the larger tile sizes available the assets must be provided.
            secondaryTile.VisualElements.Wide310x150Logo = tileImage; //wide310x150Logo;
            secondaryTile.VisualElements.Square310x310Logo = tileImage; //square310x310Logo;
#endif

            // Like the background color, the square30x30 logo is inherited from the parent application tile by default. 
            // Let's override it, just to see how that's done.
            Debug.WriteLine("Setting Square30x30Logo");
            secondaryTile.VisualElements.Square30x30Logo = tileImage; //square30x30Logo;

            // The display of the secondary tile name can be controlled for each tile size.
            // The default is false.
            Debug.WriteLine("Setting Show Names");
            secondaryTile.VisualElements.ShowNameOnSquare150x150Logo = true;

#if WINDOWS_APP
            secondaryTile.VisualElements.ShowNameOnWide310x150Logo = true;
            secondaryTile.VisualElements.ShowNameOnSquare310x310Logo = true;
#endif

            // Specify a foreground text value.
            // The tile background color is inherited from the parent unless a separate value is specified.
            Debug.WriteLine("Setting Text Foreground Value");
            secondaryTile.VisualElements.ForegroundText = ForegroundText.Dark;

            // Set this to false if roaming doesn't make sense for the secondary tile.
            // The default is true;
            Debug.WriteLine("Setting Roaming");
            secondaryTile.RoamingEnabled = false;

            // OK, the tile is created and we can now attempt to pin the tile.
#if WINDOWS_APP
            Debug.WriteLine("Pinning Tile");
            // Note that the status message is updated when the async operation to pin the tile completes.
            bool isPinned = await secondaryTile.RequestCreateForSelectionAsync(GetElementRect((FrameworkElement)sender), Windows.UI.Popups.Placement.Below);

            if (isPinned) {
                Debug.WriteLine("Success!");
                return true; //rootPage.NotifyUser("Secondary tile successfully pinned.", NotifyType.StatusMessage);
            }
            else {
                Debug.WriteLine("Failed");
                return false; //rootPage.NotifyUser("Secondary tile not pinned.", NotifyType.ErrorMessage);
            }
#endif

#if WINDOWS_PHONE_APP
            // Since pinning a secondary tile on Windows Phone will exit the app and take you to the start screen, any code after 
            // RequestCreateForSelectionAsync or RequestCreateAsync is not guaranteed to run.  For an example of how to use the OnSuspending event to do
            // work after RequestCreateForSelectionAsync or RequestCreateAsync returns, see Scenario9_PinTileAndUpdateOnSuspend in the SecondaryTiles.WindowsPhone project.
            Debug.WriteLine("Pinning Tile");
            bool result = await secondaryTile.RequestCreateAsync();

            Debug.WriteLine("Result: " + result.ToString());
            return result;
#endif
        }

        public static async Task<bool> RemoveTile(string tileID, Rect popupRect, Placement placement = Placement.Default)
        {
            if (DoesTileExist(tileID)) {
                // First prepare the tile to be unpinned
                SecondaryTile secondaryTile = new SecondaryTile(tileID);

                // Now make the delete request.
                bool isUnpinned = await secondaryTile.RequestDeleteForSelectionAsync(popupRect, placement);
                if (isUnpinned) {
                    try {
                        await StorageTools.DeleteFile(StorageTools.StorageConsts.TileFolder + "\\" +
                                                      tileID + ".jpg",
                                                      Windows.Storage.StorageDeleteOption.PermanentDelete);
                    }
                    catch (Exception) { }

                    return true;
                }
                else {
                    return false;
                }
            }
            else {
                return false;
            }
        }
        #endregion
    }
}
