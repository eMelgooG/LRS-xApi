using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using xApi.Data.Helpers;
using xApi.Data.Json;

namespace xApi.Data
{
    public class ContextActivities : JsonModel
    {
        public ContextActivities() { }

        public ContextActivities(JsonString jsonString) : this(jsonString.ToJToken(), ApiVersion.GetLatest()) { }

        public ContextActivities(JToken contextActivities, ApiVersion version)
        {
            GuardType(contextActivities, JTokenType.Object);

            var parent = contextActivities["parent"];
            if (parent != null)
            {
                Parent = new ActivityCollection(parent, version);
            }

            var category = contextActivities["category"];
            if (category != null)
            {
                Category = new ActivityCollection(category, version);
            }

            var grouping = contextActivities["grouping"];
            if (grouping != null)
            {
                Grouping = new ActivityCollection(grouping, version);
            }

            var other = contextActivities["other"];
            if (other != null)
            {
                Other = new ActivityCollection(other, version);
            }

            GuardAdditionalProperties((JObject)contextActivities, "parent", "category", "grouping", "other");
        }

        /// <summary>
        /// Parent: an Activity with a direct relation to the Activity which is the Object of the Statement.
        /// In almost all cases there is only one sensible parent or none, not multiple.
        /// For example: a Statement about a quiz question would have the quiz as its parent Activity.
        /// </summary>
        public ActivityCollection Parent { get; set; }

        /// <summary>
        /// Category: an Activity used to categorize the Statement. "Tags" would be a synonym.
        /// Category SHOULD be used to indicate a profile of xAPI behaviors, as well as other categorizations.
        /// For example: Anna attempts a biology exam, and the Statement is tracked using the cmi5 profile.
        /// The Statement's Activity refers to the exam, and the category is the cmi5 profile.
        /// </summary>
        public ActivityCollection Category { get; set; }

        /// <summary>
        /// Grouping: an Activity with an indirect relation to the Activity which is the Object of the Statement.
        /// For example: a course that is part of a qualification. The course has several classes.
        /// The course relates to a class as the parent, the qualification relates to the class as the grouping.
        /// </summary>
        public ActivityCollection Grouping { get; set; }

        /// <summary>
        /// Other: a contextActivity that doesn't fit one of the other properties.
        /// For example: Anna studies a textbook for a biology exam. The Statement's Activity refers to the textbook, and the exam is a contextActivity of type other.
        /// </summary>
        public ActivityCollection Other { get; set; }

        public override bool Equals(object obj)
        {
            var activities = obj as ContextActivities;
            return activities != null &&
                   EqualityComparer<ActivityCollection>.Default.Equals(Parent, activities.Parent) &&
                   EqualityComparer<ActivityCollection>.Default.Equals(Category, activities.Category) &&
                   EqualityComparer<ActivityCollection>.Default.Equals(Grouping, activities.Grouping) &&
                   EqualityComparer<ActivityCollection>.Default.Equals(Other, activities.Other);
        }

        public override int GetHashCode()
        {
            var hashCode = -682935721;
            hashCode = hashCode * -1521134295 + EqualityComparer<ActivityCollection>.Default.GetHashCode(Parent);
            hashCode = hashCode * -1521134295 + EqualityComparer<ActivityCollection>.Default.GetHashCode(Category);
            hashCode = hashCode * -1521134295 + EqualityComparer<ActivityCollection>.Default.GetHashCode(Grouping);
            hashCode = hashCode * -1521134295 + EqualityComparer<ActivityCollection>.Default.GetHashCode(Other);
            return hashCode;
        }

        public override JToken ToJToken(ApiVersion version, ResultFormat format)
        {
            var jobj = new JObject();
            if (Parent != null)
            {
                jobj["parent"] = Parent.ToJToken(version, format);
            }
            if (Category != null)
            {
                jobj["category"] = Category.ToJToken(version, format);
            }
            if (Grouping != null)
            {
                jobj["grouping"] = Grouping.ToJToken(version, format);
            }
            if (Other != null)
            {
                jobj["other"] = Other.ToJToken(version, format);
            }
            return jobj;
        }

        public static bool operator ==(ContextActivities activities1, ContextActivities activities2)
        {
            return EqualityComparer<ContextActivities>.Default.Equals(activities1, activities2);
        }

        public static bool operator !=(ContextActivities activities1, ContextActivities activities2)
        {
            return !(activities1 == activities2);
        }
    }
}