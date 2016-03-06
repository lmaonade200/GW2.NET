﻿//---------------------------------------------------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if the code is regenerated.
// </auto-generated>
// <copyright file="SkinDataModel.cs" company="GW2.NET Coding Team">
//     This product is licensed under the GNU General Public License version 2 (GPLv2).
//     See the License in the project root folder or the following page: http://www.gnu.org/licenses/gpl-2.0.html
// </copyright>
//---------------------------------------------------------------------------------------------------------------------------

namespace GW2NET.Items.Converter
{
    using System;

    using GW2NET.Common;
    using GW2NET.Items.ApiModels;
    using GW2NET.Items.Trinkets;

    /// <summary>Convertes a <see cref="ItemDataModel"/> into a <see cref="Trinket"/> object.</summary>
    public sealed partial class TrinketConverter : IConverter<ItemDataModel, Trinket>
	{
	    private readonly ITypeConverterFactory<ItemDataModel, Trinket> converterFactory;

		/// <summary>Initalizes a new instance of the <see cref="TrinketConverter"/> class.</summary>
        /// <param name="converterFactory">The <see cref="ITypeConverterFactory{TSource,TTarget}"/>.</param>
		private TrinketConverter(ITypeConverterFactory<ItemDataModel, Trinket> converterFactory)
		{
		    if (converterFactory == null)
    		{
    		    throw new ArgumentNullException(nameof(converterFactory));
    		}

		    this.converterFactory = converterFactory;
		}

		 /// <inheritdoc />
        public Trinket Convert(ItemDataModel value, object state)
		{
		    if (value == null)
    		{
    		    throw new ArgumentNullException(nameof(value));
    		}

			string discriminator = value.Details.Type;
			var converter = this.converterFactory.Create(discriminator);
			var entity = converter.Convert(value, value);
			this.Merge(entity, value, state);
			return entity;
		}

		// Implement this method in a buddy class to set properties that are specific to 'Trinket' (if any)
    	partial void Merge(Trinket entity, ItemDataModel dto, object state);

		/*
		// Use this template
		public partial class TrinketConverter
		{
		    partial void Merge(Trinket entity, ItemDataModel dto, object state)
			{
			    throw new NotImplementedException();
			}
		}
		*/
	}

#region Accessory
    /// <summary>Converts objects of type <see cref="ItemDataModel"/> to objects of type <see cref="Accessory"/>.</summary>
    public sealed partial class AccessoryConverter : IConverter<ItemDataModel, Trinket>
    {
	    /// <inheritdoc />
        public Trinket Convert(ItemDataModel value, object state)
        {
    		var entity = new Accessory();
            this.Merge(entity, value, state);
    		return entity;
        }

    	// Implement this method in a buddy class to set properties that are specific to 'Accessory' (if any)
    	partial void Merge(Accessory entity, ItemDataModel dto, object state);

		/*
		// Use this template
		public partial class AccessoryConverter
		{
		    partial void Merge(Accessory entity, ItemDataModel dto, object state)
			{
			    throw new NotImplementedException();
			}
		}
		*/
    }
#endregion

#region Amulet
    /// <summary>Converts objects of type <see cref="ItemDataModel"/> to objects of type <see cref="Amulet"/>.</summary>
    public sealed partial class AmuletConverter : IConverter<ItemDataModel, Trinket>
    {
	    /// <inheritdoc />
        public Trinket Convert(ItemDataModel value, object state)
        {
    		var entity = new Amulet();
            this.Merge(entity, value, state);
    		return entity;
        }

    	// Implement this method in a buddy class to set properties that are specific to 'Amulet' (if any)
    	partial void Merge(Amulet entity, ItemDataModel dto, object state);

		/*
		// Use this template
		public partial class AmuletConverter
		{
		    partial void Merge(Amulet entity, ItemDataModel dto, object state)
			{
			    throw new NotImplementedException();
			}
		}
		*/
    }
#endregion

#region Ring
    /// <summary>Converts objects of type <see cref="ItemDataModel"/> to objects of type <see cref="Ring"/>.</summary>
    public sealed partial class RingConverter : IConverter<ItemDataModel, Trinket>
    {
	    /// <inheritdoc />
        public Trinket Convert(ItemDataModel value, object state)
        {
    		var entity = new Ring();
            this.Merge(entity, value, state);
    		return entity;
        }

    	// Implement this method in a buddy class to set properties that are specific to 'Ring' (if any)
    	partial void Merge(Ring entity, ItemDataModel dto, object state);

		/*
		// Use this template
		public partial class RingConverter
		{
		    partial void Merge(Ring entity, ItemDataModel dto, object state)
			{
			    throw new NotImplementedException();
			}
		}
		*/
    }
#endregion

#region UnknownTrinket
    /// <summary>Converts objects of type <see cref="ItemDataModel"/> to objects of type <see cref="UnknownTrinket"/>.</summary>
    public sealed partial class UnknownTrinketConverter : IConverter<ItemDataModel, Trinket>
    {
	    /// <inheritdoc />
        public Trinket Convert(ItemDataModel value, object state)
        {
    		var entity = new UnknownTrinket();
            this.Merge(entity, value, state);
    		return entity;
        }

    	// Implement this method in a buddy class to set properties that are specific to 'UnknownTrinket' (if any)
    	partial void Merge(UnknownTrinket entity, ItemDataModel dto, object state);

		/*
		// Use this template
		public partial class UnknownTrinketConverter
		{
		    partial void Merge(UnknownTrinket entity, ItemDataModel dto, object state)
			{
			    throw new NotImplementedException();
			}
		}
		*/
    }
#endregion

}
