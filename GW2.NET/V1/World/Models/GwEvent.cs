﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GwEvent.cs" company="GW2.Net Coding Team">
//   This product is licensed under the GNU General Public License version 2 (GPLv2) as defined on the following page: http://www.gnu.org/licenses/gpl-2.0.html
// </copyright>
// <summary>
//   Defines the GwEvent type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;

using Newtonsoft.Json;

namespace GW2DotNET.V1.World.Models
{
    /// <summary>
    /// Represents an event in the game.
    /// </summary>
    public class GwEvent
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GwEvent"/> struct.
        /// </summary>
        /// <param name="worldId">The world id.</param>
        /// <param name="mapId">The map id.</param>
        /// <param name="eventId">The event id.</param>
        /// <param name="state">The state.</param>
        /// <param name="name">The name.</param>
        [JsonConstructor()]
        public GwEvent(int worldId, int mapId, Guid eventId, GwEventState state, string name)
        {
            this.WorldId = worldId;
            this.MapId = mapId;
            this.EventId = eventId;
            this.State = state;
            this.Name = name;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GwEvent"/> struct.
        /// This is the construction that should be used when we've
        /// resolved the IDs.
        /// </summary>
        /// <param name="worldId"></param>
        /// <param name="mapId"></param>
        /// <param name="eventId"></param>
        /// <param name="state"></param>
        /// <param name="name"></param>
        /// <param name="world"></param>
        /// <param name="map"></param>
        public GwEvent(int worldId, int mapId, Guid eventId, GwEventState state, string name, GwWorld world, GwMap map) :
            this(worldId, mapId, eventId, state, name)
        {
            this.World = world;
            this.Map = map;
        }

        /// <summary>
        /// Gets the name of the event.
        /// </summary>
        [JsonProperty("name")]
        public string Name
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the world id.
        /// </summary>
        [JsonProperty("world_id")]
        internal int WorldId
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the world
        /// </summary>
        public GwWorld World
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the map id.
        /// </summary>
        [JsonProperty("map_id")]
        internal int MapId
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the map
        /// </summary>
        public GwMap Map
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the event id.
        /// </summary>
        [JsonProperty("event_id")]
        public Guid EventId
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the state.
        /// </summary>
        [JsonProperty("state")]
        public GwEventState State
        {
            get;
            private set;
        }

        /// <summary>
        /// Determines whether two specified instances of <see crdef="Map"/> are equal.
        /// </summary>
        /// <param name="a">The first object to compare.</param>param>
        /// <param name="b">The second object to compare. </param>
        /// <returns>true if mapA and mapB represent the same map; otherwise, false.</returns>
        public static bool operator ==(GwEvent a, GwEvent b)
        {
            return a.EventId == b.EventId;
        }

        /// <summary>
        /// Determines whether two specified instances of <see crdef="Map"/> are not equal.
        /// </summary>
        /// <param name="a">The first object to compare.</param>param>
        /// <param name="b">The second object to compare. </param>
        /// <returns>true if mapA and mapB do not represent the same map; otherwise, false.</returns>
        public static bool operator !=(GwEvent a, GwEvent b)
        {
            return a.EventId != b.EventId;
        }

        /// <summary>
        /// Indicates whether this instance and a specified object are equal.
        /// </summary>
        /// <returns>true if <paramref name="obj"/> and this instance are the same type and represent the same value; otherwise, false.</returns>
        /// <param name="obj">Another object to compare to.</param>
        public override bool Equals(object obj)
        {
            return obj is GwEvent && this == (GwEvent)obj;
        }

        /// <summary>
        /// Indicates whether this instance and a specified <see cref="Map"/> are equal.
        /// </summary>
        /// <returns>true if <paramref name="obj"/> and this instance are the same type and represent the same value; otherwise, false.</returns>
        /// <param name="obj">Another object to compare to. </param>
        public bool Equals(GwEvent obj)
        {
            return this.EventId == obj.EventId;
        }

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>A 32-bit signed integer that is the hash code for this instance.</returns>
        public override int GetHashCode()
        {
            return this.EventId.GetHashCode();
        }
    }
}
