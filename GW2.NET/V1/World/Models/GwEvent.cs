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
    public struct GwEvent
    {
        /// <summary>
        /// The event id. This field is readonly.
        /// </summary>
        private Guid eventId;

        /// <summary>
        /// Initializes a new instance of the <see cref="GwEvent"/> struct.
        /// </summary>
        /// <param name="worldId">The world id.</param>
        /// <param name="mapId">The map id.</param>
        /// <param name="eventId">The event id.</param>
        /// <param name="state">The state.</param>
        /// <param name="name">The name.</param>
        public GwEvent(int worldId, int mapId, Guid eventId, GwEventState state, string name)
            : this()
        {
            this.WorldId = worldId;
            this.MapId = mapId;
            this.eventId = eventId;
            this.State = state;
            this.Name = name;
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
        public int WorldId
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the map id.
        /// </summary>
        [JsonProperty("map_id")]
        public int MapId
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
            get
            {
                return this.eventId;
            }

            private set
            {
                this.eventId = value;
            }
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
        /// <param name="mapA">The first object to compare.</param>param>
        /// <param name="mapB">The second object to compare. </param>
        /// <returns>true if mapA and mapB represent the same map; otherwise, false.</returns>
        public static bool operator ==(GwEvent mapA, GwEvent mapB)
        {
            return mapA.eventId == mapB.eventId;
        }

        /// <summary>
        /// Determines whether two specified instances of <see crdef="Map"/> are not equal.
        /// </summary>
        /// <param name="mapA">The first object to compare.</param>param>
        /// <param name="mapB">The second object to compare. </param>
        /// <returns>true if mapA and mapB do not represent the same map; otherwise, false.</returns>
        public static bool operator !=(GwEvent mapA, GwEvent mapB)
        {
            return mapA.eventId != mapB.eventId;
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
            return this.eventId == obj.eventId;
        }

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>A 32-bit signed integer that is the hash code for this instance.</returns>
        public override int GetHashCode()
        {
            return this.eventId.GetHashCode();
        }
    }
}
