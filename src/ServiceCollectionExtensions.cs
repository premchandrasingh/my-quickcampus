///////////////////////////////////////////////////////////////////////////////////////////////////
// <copyright file="ServiceCollectionExtensions.cs" company="Cision US Inc (Cision)">
//     Copyright (c) Cision US Inc (Cision). All rights reserved.
// </copyright>
///////////////////////////////////////////////////////////////////////////////////////////////////

namespace My.QuickCampus
{
    public static class ServiceCollectionExtensions
    {
        public static T ConfigureAndGet<T>(this IConfiguration configuration, IServiceCollection services, string sectionName) where T : class
        {
            if (string.IsNullOrWhiteSpace(sectionName))
                throw new ArgumentException("Section name cannot be empty", nameof(sectionName));

            var section = configuration.GetSection(sectionName);
            services.AddOptions().Configure<T>(section);
            return section.Get<T>();
        }
    }
}
