// MIT License
// 
// Copyright (c) 2016 FXGuild
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and
// associated documentation files (the "Software"), to deal in the Software without restriction,
// including without limitation the rights to use, copy, modify, merge, publish, distribute,
// sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all copies or
// substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT
// NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;

// ReSharper disable StaticMemberInGenericType

namespace FXGuild.Common.Misc
{
   public abstract class JavaLikeEnum<T> where T : class
   {
      #region Runtime constants

      private static readonly List<T> CONSTANTS;

      private static readonly Dictionary<Type, Dictionary<string, T>> SUBTYPE_TO_NAME_TO_CONSTANT;

      #endregion

      #region Private fields

      private uint m_Ordinal;

      #endregion

      #region Properties

      public string Name { get; private set; }

      #endregion

      #region Constructors

      static JavaLikeEnum()
      {
         CONSTANTS = new List<T>();
         SUBTYPE_TO_NAME_TO_CONSTANT = new Dictionary<Type, Dictionary<string, T>>();

         if (typeof(T).IsSealed)
            ExtractConstants(typeof(T));
      }

      #endregion

      #region Static methods

      public static bool TryParse<TArg>(string a_Name, out TArg a_Value) where TArg : class, T
      {
         T value;
         bool found = SUBTYPE_TO_NAME_TO_CONSTANT[typeof(TArg)].TryGetValue(a_Name, out value);
         a_Value = found ? value as TArg : null;
         return found;
      }

      public static ReadOnlyCollection<T> Values()
      {
         return CONSTANTS.AsReadOnly();
      }

      public static ReadOnlyCollection<TArg> Values<TArg>() where TArg : class, T
      {
         var submap = SUBTYPE_TO_NAME_TO_CONSTANT[typeof(TArg)] as Dictionary<string, TArg>;
         // ReSharper disable once PossibleNullReferenceException
         return submap.Values.ToList().AsReadOnly();
      }

      private static void ExtractConstants(Type a_Type)
      {
         // Check that type only has private constructor
         if (!a_Type.GetConstructors().All(a_C => a_C.IsPrivate))
            throw new JavaLikeEnumException("Class \"" + typeof(T) + "\" must " +
                                            "only have private constructors");

         // Look for constant instances of the type
         var constantsInfo = typeof(T).GetFields(BindingFlags.Public |
                                                 BindingFlags.Static |
                                                 BindingFlags.FlattenHierarchy)
            .Where(a_M => a_M.IsInitOnly && (a_M.DeclaringType == a_Type)).ToArray();
         if (constantsInfo.Length < 2)
            throw new JavaLikeEnumException("Class \"" + a_Type + "\" must have " +
                                            "at least two public static readonly \" " +
                                            "+ a_Type + \" constants defined");

         a_Type.TypeInitializer.Invoke(null, null);

         // Add constants to data structures
         var submap = new Dictionary<string, T>();
         foreach (var info in constantsInfo)
         {
            var value = info.GetValue(null) as JavaLikeEnum<T>;
            if (value == null)
               throw new JavaLikeEnumException("Unexpected error");
            value.m_Ordinal = (uint) CONSTANTS.Count;
            value.Name = info.Name;

            CONSTANTS.Add(value as T);
            submap.Add(info.Name, value as T);
         }
         SUBTYPE_TO_NAME_TO_CONSTANT.Add(a_Type, submap);
      }

      #endregion

      #region Methods

      public T Next()
      {
         return CONSTANTS[(int) (m_Ordinal + 1) % CONSTANTS.Count];
      }

      public T Previous()
      {
         return CONSTANTS[(int) (m_Ordinal + CONSTANTS.Count - 1) % CONSTANTS.Count];
      }

      #endregion
   }
}
