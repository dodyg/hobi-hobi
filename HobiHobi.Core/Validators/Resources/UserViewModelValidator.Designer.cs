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
    public class UserViewModelValidator {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal UserViewModelValidator() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("HobiHobi.Core.Validators.Resources.UserViewModelValidator", typeof(UserViewModelValidator).Assembly);
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
        public static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Please enter a proper email.
        /// </summary>
        public static string CorrectEmailFormat {
            get {
                return ResourceManager.GetString("CorrectEmailFormat", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Your passwords do not match.
        /// </summary>
        public static string MatchPassword {
            get {
                return ResourceManager.GetString("MatchPassword", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Please make sure your password is between 10 and 20 characters long.
        /// </summary>
        public static string RangePassword {
            get {
                return ResourceManager.GetString("RangePassword", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to This email has been used. Did you forget your account? Please use another email account for registration.
        /// </summary>
        public static string RepeatEmail {
            get {
                return ResourceManager.GetString("RepeatEmail", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Email is required.
        /// </summary>
        public static string ReqEmail {
            get {
                return ResourceManager.GetString("ReqEmail", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to First Name is required.
        /// </summary>
        public static string ReqFirstName {
            get {
                return ResourceManager.GetString("ReqFirstName", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Last Name is required.
        /// </summary>
        public static string ReqLastName {
            get {
                return ResourceManager.GetString("ReqLastName", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Password is required.
        /// </summary>
        public static string ReqPassword {
            get {
                return ResourceManager.GetString("ReqPassword", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Please repeat your password.
        /// </summary>
        public static string ReqRepeatPassword {
            get {
                return ResourceManager.GetString("ReqRepeatPassword", resourceCulture);
            }
        }
    }
}
