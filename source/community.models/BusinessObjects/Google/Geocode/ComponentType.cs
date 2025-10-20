// ReSharper disable InconsistentNaming
namespace community.models.BusinessObjects.Google.Geocode;

/// <summary>
/// AddressModel types and address component types
/// </summary>
public enum ComponentType
{
    /// <summary>
    /// indicates a precise street address.
    /// </summary>
    street_address,
    
    /// <summary>
    /// Street Number
    /// </summary>
    street_number,

    /// <summary>
    /// indicates a named route (such as "US 101").
    /// </summary>
    route,
    
    /// <summary>
    /// indicates a major intersection, usually of two major roads.
    /// </summary>
    intersection,
    
    /// <summary>
    /// indicates a political entity. Usually, this type indicates a polygon of some civil administration.
    /// </summary>
    political,
    
    /// <summary>
    /// indicates the national political entity, and is typically the highest order type returned by the Geocoder.
    /// </summary>
    country,
    
    /// <summary>
    /// indicates a first-order civil entity below the country level. Within the United States, these administrative levels are states. Not all nations exhibit these administrative levels. In most cases, administrative_area_level_1 short names will closely match ISO 3166-2 subdivisions and other widely circulated lists; however this is not guaranteed as our geocoding results are based on a variety of signals and location data.
    /// </summary>
    administrative_area_level_1,
    
    /// <summary>
    ///  indicates a second-order civil entity below the country level. Within the United States, these administrative levels are counties. Not all nations exhibit these administrative levels.
    /// </summary>
    administrative_area_level_2,
    
    /// <summary>
    /// indicates a third-order civil entity below the country level. This type indicates a minor civil division. Not all nations exhibit these administrative levels.
    /// </summary>
    administrative_area_level_3,
    
    /// <summary>
    ///  indicates a fourth-order civil entity below the country level. This type indicates a minor civil division. Not all nations exhibit these administrative levels.
    /// </summary>
    administrative_area_level_4,
    
    /// <summary>
    ///  indicates a fourth-order civil entity below the country level. This type indicates a minor civil division. Not all nations exhibit these administrative levels.
    /// </summary>
    administrative_area_level_5,

    /// <summary>
    ///  indicates a fourth-order civil entity below the country level. This type indicates a minor civil division. Not all nations exhibit these administrative levels.
    /// </summary>
    administrative_area_level_6,
    
    /// <summary>
    ///  indicates a fourth-order civil entity below the country level. This type indicates a minor civil division. Not all nations exhibit these administrative levels.
    /// </summary>
    administrative_area_level_7,
    
    /// <summary>
    /// indicates a commonly-used alternative name for the entity.
    /// </summary>
    colloquial_area,
    
    /// <summary>
    ///  indicates an incorporated city or town political entity.
    /// </summary>
    locality,
    
    /// <summary>
    /// indicates a first-order civil entity below a locality. For some locations may receive one of the additional types: sublocality_level_1 to sublocality_level_5. Each sublocality level is a civil entity. Larger numbers indicate a smaller geographic area.
    /// </summary>
    sublocality,
    
    /// <summary>
    /// indicates a named neighborhood.
    /// </summary>
    neighborhood,
    
    /// <summary>
    ///  indicates a named location, usually a building or collection of buildings with a common name.
    /// </summary>
    premise,
    
    /// <summary>
    ///  indicates an addressable entity below the premise level, such as an apartment, unit, or suite.
    /// </summary>
    subpremise,
    
    /// <summary>
    /// indicates an encoded location reference, derived from latitude and longitude. Plus codes can be used as a replacement for street addresses in places where they do not exist (where buildings are not numbered or streets are not named). See https://plus.codes for details.
    /// </summary>
    plus_code,
    
    /// <summary>
    ///  indicates a postal code as used to address postal mail within the country.
    /// </summary>
    postal_code,
    
    /// <summary>
    /// indicates a prominent natural feature.
    /// </summary>
    natural_feature,
    
    /// <summary>
    /// indicates an airport
    /// </summary>
    airport,
    
    /// <summary>
    /// indicates a named park
    /// </summary>
    park,
    
    /// <summary>
    /// indicates a named point of interest. Typically, these "POI"s are prominent local entities that don't easily fit in another category, such as "Empire State Building" or "Eiffel Tower".
    /// </summary>
    point_of_interest,
    
    /// <summary>
    /// last 4 of postalcode 
    /// </summary>
    postal_code_suffix
}