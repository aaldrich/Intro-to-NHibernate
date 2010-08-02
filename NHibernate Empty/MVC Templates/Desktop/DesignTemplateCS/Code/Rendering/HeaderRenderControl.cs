using System;
using System.Data;
using System.Linq;
using System.Web;
using System.Xml.Linq;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace Rendering {
    
    /// <summary>
    /// Contains data to use for rendering a header
    /// </summary>
    public class HeaderRenderControl {

        #region Constants

        private const string TAB_PATH_AND_PREFIX = "~/content/tabs/tab";
        private const string TEMPLATE_TAB_PATH_AND_PREFIX = "~/content/templateimages/tab";
        private const string TAB_SEPARATOR = "-";
        private const string TAB_FILE_TYPE = ".jpg";
        private const string TAB_LAST_POSTFIX = "_last";
        private const int MAXIMUM_TABS = 4;
        private const long TAB_IMAGE_QUALITY = 90L;

        private const string EXCEPTION_TOO_MANY_TAGS = "There are too many tags found. You can only have up to 4 tags using this control.";
        private const string EXCEPTION_TAG_NOT_IN_COLLECTION = "The provided tab wasn't found. Are you sure this tab was added to this header?";
        private const string EXCEPTION_TEMPLATE_IMAGE_MISSING = "Missing the Tab template image for '{0}'.";

        #endregion

        #region Tab Position Information

        //Holds tab information for each "center" of the tabs to
        //help align the tabs correctly. The first value is the 
        //tab if it is a shared tab. The second value is if it
        //is the last tab
        private readonly static PointF[] _TabPositions = new PointF[] {
            new PointF(88,40),
            new PointF(58,41),
            new PointF(59,40),
            new PointF(54,40)
        };

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a header without any tabs
        /// </summary>
        public HeaderRenderControl(Font font, LogoDetails logo) {
            this._SetupHeaderControl(font, logo, new TabDetails[] { });
        }

        /// <summary>
        /// Creates a header with one (1) tabs
        /// </summary>
        public HeaderRenderControl(Font font, LogoDetails logo, TabDetails tab1) {
            this._SetupHeaderControl(font, logo, new TabDetails[] { tab1 });
        }

        /// <summary>
        /// Creates a header with two (2) tabs
        /// </summary>
        public HeaderRenderControl(Font font, LogoDetails logo, TabDetails tab1, TabDetails tab2) {
            this._SetupHeaderControl(font, logo, new TabDetails[] { tab1, tab2 });
        }

        /// <summary>
        /// Creates a header with three (3) tabs
        /// </summary>
        public HeaderRenderControl(Font font, LogoDetails logo, TabDetails tab1, TabDetails tab2, TabDetails tab3) {
            this._SetupHeaderControl(font, logo, new TabDetails[] { tab1, tab2, tab3 });
        }

        /// <summary>
        /// Creates a header with four (4) tabs
        /// </summary>
        public HeaderRenderControl(Font font, LogoDetails logo, TabDetails tab1, TabDetails tab2, TabDetails tab3, TabDetails tab4) {
            this._SetupHeaderControl(font, logo, new TabDetails[] { tab1, tab2, tab3, tab4 });
        }

        /// <summary>
        /// Performs the actual work setting up the header
        /// </summary>
        public void _SetupHeaderControl(Font font, LogoDetails logo, params TabDetails[] tabs) {
            this.TabFont = font;
            this.Logo = logo;
            this.Tabs = tabs;
        }

        #endregion

        #region Properties

        /// <summary>
        /// The font used for rendering the tab
        /// </summary>
        public Font TabFont { get; private set; }

        /// <summary>
        /// Holds the information on rendering the tabs
        /// </summary>
        public TabDetails[] Tabs { get; private set; }

        /// <summary>
        /// Contains information on how to render the logo
        /// </summary>
        public LogoDetails Logo { get; private set; }

        #endregion

        #region URL Preparation

        /// <summary>
        /// Creates the correct Url for an image and if needed, renders the tab
        /// </summary>
        /// <remarks>
        /// The URL is determined by generating a hashcode from the name provided
        /// this code is checked each time the page renders (incase the images 
        /// change).
        /// </remarks>
        public string GetTabUrl(TabDetails tab) {

            //make sure this tab doesn't exceed the maximum tabs            
            int tabIndex = this.Tabs.ToList().IndexOf(tab);
            if (tabIndex == MAXIMUM_TABS) {
                throw new ArgumentOutOfRangeException(HeaderRenderControl.EXCEPTION_TOO_MANY_TAGS);
            }

            //make sure any tab was found to begin with
            if (tabIndex == -1) {
                throw new ArgumentException(HeaderRenderControl.EXCEPTION_TAG_NOT_IN_COLLECTION);
            }

            //create an identifier for this image using the index
            //of the tab and the text to display. This will determine
            //if this tab has already been rendered or not
            bool isLastTab = (tabIndex == Tabs.Length - 1);
            string identifier = string.Concat(
                tabIndex, //where the tab is
                tab.Text, //what the tag says
                isLastTab.ToString() //string true or false for it's end point
                )
                .GetHashCode()
                .ToString();
            
            //create the image name by appending the prefix and path
            //onto a hashcode of the file name. 
            string imagePath = string.Concat(
                HeaderRenderControl.TAB_PATH_AND_PREFIX,                
                identifier,
                HeaderRenderControl.TAB_FILE_TYPE
                );

            //check for this image - if it does not exist, remove it now
            string fullPath = HttpContext.Current.Server.MapPath(imagePath);
            if (!File.Exists(fullPath)) {

                //render the tab image
                this._RenderTab(tab, fullPath, tabIndex, isLastTab);
            }

            //return the path to this file
            return VirtualPathUtility.ToAbsolute(
                imagePath,
                HttpContext.Current.Request.ApplicationPath
                );
        }

        #endregion

        #region Image Rendering

        /// <summary>
        /// Renders and saves a tab to the tab storage directory
        /// </summary>
        private void _RenderTab(TabDetails tab, string path, int index, bool isLastTab) {

            //determine the correct path to this tab            
            string templateTabPath = HttpContext.Current.Server.MapPath(
                string.Concat(
                    HeaderRenderControl.TEMPLATE_TAB_PATH_AND_PREFIX,
                    (index + 1),
                    (isLastTab ? HeaderRenderControl.TAB_LAST_POSTFIX : string.Empty),
                    HeaderRenderControl.TAB_FILE_TYPE
                ));

            //make sure the image is available
            if (!File.Exists(templateTabPath)) {
                throw new FileNotFoundException(
                    string.Format(
                        HeaderRenderControl.EXCEPTION_TEMPLATE_IMAGE_MISSING, 
                        templateTabPath
                        ));
            }

            //load the image and the coordinates for this the text
            Image template = Image.FromFile(templateTabPath);
            Graphics render = Graphics.FromImage(template);

            //get the size and positions for this text
            SizeF size = render.MeasureString(tab.Text, this.TabFont);
            PointF center = HeaderRenderControl._TabPositions[index];

            //render the text at the appropriate position
            render.DrawString(
                tab.Text,
                this.TabFont,
                new SolidBrush(Color.Black),
                new PointF(
                    center.X - (size.Width / 2),
                    center.Y - (size.Height / 2)
                    ));

            //get the jpeg encoder
            ImageCodecInfo encoder = ImageCodecInfo.GetImageEncoders()
                .Where(o => o.FormatID == ImageFormat.Jpeg.Guid)
                .First();
            
            //set the image quality
            EncoderParameters parameters = new EncoderParameters(1);
            parameters.Param[0] = new EncoderParameter(
                Encoder.Quality, 
                HeaderRenderControl.TAB_IMAGE_QUALITY
                );

            //save the final image for use
            template.Save(path, encoder, parameters);

            //clean up the rendering classes
            template.Dispose();
            render.Dispose();

        }

        #endregion

    }

    /// <summary>
    /// Information for how to render the site logo 
    /// </summary>
    public class LogoDetails {

        /// <summary>
        /// The text to use with the tab image for the "alt" attribute
        /// </summary>
        public string AlternateText;

        /// <summary>
        /// The text to insert into the "title" attribute
        /// </summary>
        public string Title;
        
        /// <summary>
        /// The url to use for the logo
        /// </summary>
        public string Url;

        /// <summary>
        /// Creates a new LogoDetails using the provided parameters
        /// </summary>
        public LogoDetails(string url) {            
            this.Url = url;
            this.Title = string.Empty;
            this.AlternateText = string.Empty;
        }

        /// <summary>
        /// Creates a new LogoDetails using the provided parameters
        /// </summary>
        public LogoDetails(string url, string title) {            
            this.Url = url;
            this.Title = title;
            this.AlternateText = string.Empty;
        }

        /// <summary>
        /// Creates a new LogoDetails using the provided parameters
        /// </summary>
        public LogoDetails(string url, string title, string alt) {
            this.Url = url;
            this.Title = title;            
            this.AlternateText = alt;
        }

    }

    /// <summary>
    /// Information on how to correctly render a tab within the Header control
    /// </summary>
    public class TabDetails {

        /// <summary>
        /// The text to render onto the tab itself
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// The text to use with the tab image for the "alt" attribute
        /// </summary>
        public string AlternateText { get; set; }

        /// <summary>
        /// The text to insert into the "title" attribute
        /// </summary>
        public string Title { get; set; }
        
        /// <summary>
        /// The action to use with this url
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Creates a new TabDetails using the provided parameters
        /// </summary>
        /// <param name="url">
        /// The actual url to use for the tab. If this URL requires
        /// any modification, it must be done before it is used here.
        /// </param>
        public TabDetails(string text, string url) {
            this.Text = text;
            this.Url = url;
            this.AlternateText = Text;
            this.Title = string.Empty;            
        }

        /// <summary>
        /// Creates a new TabDetails using the provided parameters
        /// </summary>
        /// <param name="url">
        /// The actual url to use for the tab. If this URL requires
        /// any modification, it must be done before it is used here.
        /// </param>
        public TabDetails(string text, string url, string title) {
            this.Text = text;
            this.Url = url;            
            this.Title = title;
            this.AlternateText = text;
        }

        /// <summary>
        /// Creates a new TabDetails using the provided parameters
        /// </summary>
        /// <param name="url">
        /// The actual url to use for the tab. If this URL requires
        /// any modification, it must be done before it is used here.
        /// </param>
        public TabDetails(string text, string url, string title, string alt) {
            this.Text = text;
            this.Url = url;
            this.Title = title;            
            this.AlternateText = alt;           
        }

    }

}
