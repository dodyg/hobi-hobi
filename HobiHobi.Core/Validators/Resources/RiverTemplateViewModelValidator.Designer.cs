﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.17929
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace HobiHobi.Core.Validators.Resources {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class RiverTemplateViewModelValidator {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal RiverTemplateViewModelValidator() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("HobiHobi.Core.Validators.Resources.RiverTemplateViewModelValidator", typeof(RiverTemplateViewModelValidator).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to CoffeeScript is required if you do not specify JavaScript.
        /// </summary>
        internal static string ReqCoffeeScript {
            get {
                return ResourceManager.GetString("ReqCoffeeScript", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to CSS is required.
        /// </summary>
        internal static string ReqCss {
            get {
                return ResourceManager.GetString("ReqCss", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Feed Template cannot be empty.
        /// </summary>
        internal static string ReqFeedTemplate {
            get {
                return ResourceManager.GetString("ReqFeedTemplate", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to JavaScript is required if you do not specify CoffeeScript.
        /// </summary>
        internal static string ReqJavaScript {
            get {
                return ResourceManager.GetString("ReqJavaScript", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Wall Template cannot be empty.
        /// </summary>
        internal static string ReqWallTemplate {
            get {
                return ResourceManager.GetString("ReqWallTemplate", resourceCulture);
            }
        }
    }
}
