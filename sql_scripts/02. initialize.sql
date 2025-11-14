CREATE EXTENSION IF NOT EXISTS citext;

drop table if exists user_address;
drop table if exists community_address;
drop table if exists "address";
drop table if exists address_type;
drop table if exists counties;
drop table if exists states;
drop table if exists countries;
drop table if exists user_community_claim;
drop table if exists report_type__user_community__subscription;
drop table if exists user_community;
drop table if exists contact_consent_log;
drop table if exists contact;
drop table if exists contact_method;
drop table if exists report_comment;
drop table if exists report_user_acknowledgement;
drop table if exists report;
drop table if exists report_type;
drop table if exists "user";
drop table if exists user_type_claim_section;
drop table if exists user_type;
drop table if exists community;
drop table if exists parcel_size_unit;
drop table if exists claim;
drop table if exists claim_section;

-- enums
drop type if exists contact_type;
drop type if exists entity_type;

-- end enums

-- functions
create or replace function utcnow() returns timestamp as $$
 select now() at time zone 'utc'
$$ language sql;

create or replace function emptyGuid() returns uuid as $$
    select cast('00000000-0000-0000-0000-000000000000' as uuid)
$$ language sql;

create or replace function uuid7() returns uuid as $$
declare
begin
	return uuid7(clock_timestamp());
end $$ language plpgsql;

create or replace function uuid7(p_timestamp timestamp with time zone) returns uuid as $$
declare

	v_time double precision := null;

	v_unix_t bigint := null;
	v_rand_a bigint := null;
	v_rand_b bigint := null;

	v_unix_t_hex varchar := null;
	v_rand_a_hex varchar := null;
	v_rand_b_hex varchar := null;

	c_milli double precision := 10^3;  -- 1 000
	c_micro double precision := 10^6;  -- 1 000 000
	c_scale double precision := 4.096; -- 4.0 * (1024 / 1000)

	c_version bigint := x'0000000000007000'::bigint; -- RFC-9562 version: b'0111...'
	c_variant bigint := x'8000000000000000'::bigint; -- RFC-9562 variant: b'10xx...'

begin

	v_time := extract(epoch from p_timestamp);

	v_unix_t := trunc(v_time * c_milli);
	v_rand_a := trunc((v_time * c_micro - v_unix_t * c_milli) * c_scale);
	-- v_rand_b := secure_random_bigint(); -- use when pgcrypto extension is installed
	v_rand_b := trunc(random() * 2^30)::bigint << 32 | trunc(random() * 2^32)::bigint;

	v_unix_t_hex := lpad(to_hex(v_unix_t), 12, '0');
	v_rand_a_hex := lpad(to_hex((v_rand_a | c_version)::bigint), 4, '0');
	v_rand_b_hex := lpad(to_hex((v_rand_b | c_variant)::bigint), 16, '0');

	return (v_unix_t_hex || v_rand_a_hex || v_rand_b_hex)::uuid;

end $$ language plpgsql;

--- end functions

--- lookups

create table countries (
    code citext not null,
    constraint pk__country__code
        primary key (code),
    name citext not null,
    iso_2 char(2) null,
    numeric_code char(3) null,
    sort_order smallint not null default 100,
    created_date timestamp without time zone not null default utcnow(),
    modified_date timestamp without time zone not null default utcnow(),
    created_by uuid null,
    modified_by uuid null,
    is_active boolean not null default true
)
with (
     fillfactor = 80
   , autovacuum_analyze_scale_factor = 0.002
   , autovacuum_vacuum_scale_factor = 0.001);

create table states (
    code citext not null,
    country_code citext not null,
    constraint pk__state__code
        primary key (code, country_code),
    constraint fk__state__country
        foreign key (country_code)
        references countries(code),
    fips_code char(2) null,
    ansi_code char(3) null,
    name citext not null,
    created_date timestamp without time zone not null default utcnow(),
    modified_date timestamp without time zone not null default utcnow(),
    created_by uuid null,
    modified_by uuid null,
    is_active boolean not null default true
)
with (
     fillfactor = 80
   , autovacuum_analyze_scale_factor = 0.002
   , autovacuum_vacuum_scale_factor = 0.001);

create table counties(
    code citext not null,
    constraint pk__county__code
        primary key (code),
    state_code citext not null,
    country_code citext not null,
    constraint fk__county__state
        foreign key (state_code, country_code)
        references states(code, country_code),
    name citext,
    created_date timestamp without time zone not null default utcnow(),
    modified_date timestamp without time zone not null default utcnow(),
    created_by uuid null,
    modified_by uuid null,
    is_active boolean not null default true
)
with (
     fillfactor = 80
   , autovacuum_analyze_scale_factor = 0.002
   , autovacuum_vacuum_scale_factor = 0.001);

create table parcel_size_unit (
    id uuid not null default uuid7(),
    constraint pk__parcel_size_unit__id
       primary key(id),
    name citext not null,
    description citext null,
    created_date timestamp without time zone not null default utcnow(),
    modified_date timestamp without time zone not null default utcnow(),
    created_by uuid null,
    modified_by uuid null,
    is_active boolean not null default true
)
with (
     fillfactor = 80
   , autovacuum_analyze_scale_factor = 0.002
   , autovacuum_vacuum_scale_factor = 0.001);

--- end lookups

create table community (
    id uuid not null default uuid7(),
    constraint pk__community__id
        primary key (id),
    name citext not null,
    description text not null,
    website citext null,
    parent_id uuid null,
    constraint fk__parent__community
        foreign key (parent_id)
        references community(id),
    s3_bucket_name varchar(255) null,
    number_of_parcels int null,
    parcel_size numeric(20,4) null,
    parcel_size_unit_id uuid null,
    number_of_residents int null,
    average_home_value money null,
    constraint fk__community__parcel_size_unit
        foreign key (parcel_size_unit_id)
        references parcel_size_unit(id),
    created_date timestamp without time zone not null default utcnow(),
    modified_date timestamp without time zone not null default utcnow(),
    created_by uuid null,
    modified_by uuid null,
    is_active boolean not null default true
)
with (
     fillfactor = 80
   , autovacuum_analyze_scale_factor = 0.002
   , autovacuum_vacuum_scale_factor = 0.001);

create table user_type (
    id uuid not null default uuid7(),
    constraint pk__user_type__id
        primary key (id),
    name citext not null,
    description citext not null,
    created_date timestamp without time zone  not null default utcnow(),
    modified_date timestamp without time zone  not null default utcnow(),
    created_by uuid null,
    modified_by uuid null,
    is_active boolean not null default true
)
with (
     fillfactor = 80
   , autovacuum_analyze_scale_factor = 0.002
   , autovacuum_vacuum_scale_factor = 0.001);

create table "user" (
    id uuid not null default uuid7(),
    constraint pk__user__id 
        primary key (id),
    user_type_id uuid not null default uuid7(),
    constraint fk__user_user__type 
        foreign key (user_type_id) 
        references user_type(id),
    username citext not null,
    password varchar(150) null,
    username_verified boolean not null default false,
    username_verified_date timestamp without time zone null,
    login_code char(6) null,
    login_code_expiration timestamp without time zone null,
    prefix citext null,
    firstname citext not null,
    lastname citext not null,
    suffix citext null,
    date_of_birth char(70),
    locked boolean not null default false,
    profile_pic varchar(255) null,
    last_login_date timestamp without time zone null,
    last_community_id uuid null,
    constraint fk__user__community
        foreign key (last_community_id)
        references community(id),
    created_date timestamp without time zone  not null default utcnow(),
    modified_date timestamp without time zone  not null default utcnow(),
    created_by uuid null,
    modified_by uuid null,
    is_active boolean not null default true
)
with (
     fillfactor = 80
   , autovacuum_analyze_scale_factor = 0.002
   , autovacuum_vacuum_scale_factor = 0.001);

create unique index ux__user__username 
    on "user"(username, is_active, locked, login_code, login_code_expiration)
    include (id, password);

create table user_community (
    user_id uuid not null default uuid7(),
    constraint fk__user_community__user
        foreign key (user_id)
        references "user"(id),
    community_id uuid not null default uuid7(), 
    constraint fk__user_community__community
        foreign key (community_id)
        references community(id),
    constraint pk__user_community__user_id__community_id
        primary key (user_id, community_id),
    verified boolean not null default false,
    verified_by uuid null,
    constraint fk__user_community__verified_by
        foreign key (verified_by)
        references "user"(id),
    verified_date timestamp without time zone null,
    created_date timestamp without time zone  not null default utcnow(),
    modified_date timestamp without time zone  not null default utcnow(),
    created_by uuid null,
    modified_by uuid null,
    is_active boolean not null default true
)
with (
     fillfactor = 80
   , autovacuum_analyze_scale_factor = 0.002
   , autovacuum_vacuum_scale_factor = 0.001);

create table contact_method (
    id uuid not null default uuid7(),
    constraint pk__contact_method__code
        primary key (id),
    community_id uuid null,
    constraint fk__contact_method__community_id
        foreign key (community_id)
        references community(id),
    name citext not null,
    contact_type citext not null,
    created_date timestamp without time zone  not null default utcnow(),
    modified_date timestamp without time zone  not null default utcnow(),
    created_by uuid null,
    modified_by uuid null,
    is_active boolean not null default true
)
with (
     fillfactor = 80
   , autovacuum_analyze_scale_factor = 0.002
   , autovacuum_vacuum_scale_factor = 0.001);

create table contact (
    id uuid not null default uuid7(),
    community_id uuid not null,
    constraint fk__user_contact_method__community
        foreign key (community_id)
        references community(id),
    user_id uuid null,
    constraint fk__user_contact_method__user
        foreign key (user_id)
        references "user"(id),
    entity_type int not null,
    constraint pk__user_contact_method__id__community_id
        primary key(id, community_id),
    contact_method_id uuid not null,
    constraint fk__user_contact_method__contact_method
        foreign key (contact_method_id)
        references contact_method(id),
    value citext not null,
    verified bool not null default false,
    verified_date timestamp without time zone not null default utcnow(),
    visible bool not null default false,
    can_contact bool not null default false,
    created_date timestamp without time zone  not null default utcnow(),
    modified_date timestamp without time zone  not null default utcnow(),
    created_by uuid null,
    modified_by uuid null,
    is_active boolean not null default true
)
with (
     fillfactor = 80
   , autovacuum_analyze_scale_factor = 0.002
   , autovacuum_vacuum_scale_factor = 0.001);

create table contact_consent_log
(
    id uuid not null default uuid7(),
    constraint pk__contact_consent_log__id
        primary key(id),
    contact_id uuid not null,
    community_id uuid not null,
    constraint fk__contact_consent_log__contact
        foreign key (contact_id, community_id)
        references contact(id, community_id),
    has_consent bool not null default true,
    consent_date timestamp without time zone not null default utcnow()
)
with (
     fillfactor = 80
   , autovacuum_analyze_scale_factor = 0.002
   , autovacuum_vacuum_scale_factor = 0.001);

create table address_type (
    id uuid not null default uuid7(),
    constraint pk__address_type__name
        primary key (id),
    community_id uuid null,
    constraint fk__address_type__community_id
        foreign key (community_id)
        references community(id),
    name citext not null,
    created_date timestamp without time zone  not null default utcnow(),
    modified_date timestamp without time zone  not null default utcnow(),
    created_by uuid null,
    modified_by uuid null,
    is_active boolean not null default true
)
with (
     fillfactor = 80
   , autovacuum_analyze_scale_factor = 0.002
   , autovacuum_vacuum_scale_factor = 0.001);

create table address (
    id uuid not null default uuid7(),
    community_id uuid not null,
    constraint fk__address__community
        foreign key (community_id)
        references community(id),
    constraint pk__address__id__community_id
        primary key (id, community_id),
    address_type_id uuid not null,
    constraint fk__address__address_type
        foreign key (address_type_id)
        references address_type(id),
    lot_number citext null,
    address_1 citext not null,
    address_2 citext null,
    address_3 citext null,
    city citext not null,
    state_code citext not null,
    postal_code citext not null,
    county_code citext null,
    place_id citext null,
    constraint fk__address__county
        foreign key (county_code)
        references counties(code),
    country_code citext not null,
    constraint fk__address__country
        foreign key (country_code)
        references countries(code),
    constraint fk__address__state
        foreign key (state_code, country_code)
        references states(code, country_code),
    timezone varchar(20) not null default 'US/Mountain',
    timezone_offset interval not null default interval '-7 hours',
    longitude numeric(9,6) null,
    latitude numeric(9,6) null,
    created_date timestamp without time zone  not null default utcnow(),
    modified_date timestamp without time zone  not null default utcnow(),
    created_by uuid null,
    modified_by uuid null,
    is_active boolean not null default true
)
with (
     fillfactor = 80
   , autovacuum_analyze_scale_factor = 0.002
   , autovacuum_vacuum_scale_factor = 0.001);

create table community_address (
    constraint pk__community_address__id__community_id
        primary key (id, community_id)
)
inherits (address);

create table user_address (
    user_id uuid not null,
    constraint fk__user_address__user
        foreign key (user_id)
        references "user"(id),
    constraint pk__user_address__user_id__community_id__address_id
        primary key (id, community_id, user_id)
)
inherits (address);

create table claim_section (
    id uuid not null default uuid7(),
    constraint pk__claim_section_id
        primary key (id),
    name citext not null,
    description citext not null,
    parent_claim_section_id uuid null,
    constraint fk_claim_section_parent
        foreign key (parent_claim_section_id)
        references claim_section(id),
    created_date timestamp without time zone  not null default utcnow(),
    modified_date timestamp without time zone  not null default utcnow(),
    created_by uuid null,
    modified_by uuid null,
    is_active boolean not null default true
)
with (
     fillfactor = 80
   , autovacuum_analyze_scale_factor = 0.002
   , autovacuum_vacuum_scale_factor = 0.001);

create table user_type_claim_section (
    user_type_id uuid not null,
    claim_section_id uuid not null,
    constraint fk__user_type__claim_section__user
        foreign key (user_type_id)
        references user_type(id),
    constraint fk__user_type__claim_section__section
        foreign key (claim_section_id)
        references claim_section(id),
    constraint pk__user_type__claim_section
        primary key (user_type_id, claim_section_id),
    created_date timestamp without time zone  not null default utcnow(),
    modified_date timestamp without time zone  not null default utcnow(),
    created_by uuid null,
    modified_by uuid null,
    is_active boolean not null default true
)
with (
     fillfactor = 80
   , autovacuum_analyze_scale_factor = 0.002
   , autovacuum_vacuum_scale_factor = 0.001);

create table claim (
    id uuid not null default uuid7(),
    constraint pk__claim_id
        primary key (id),
    claim_section_id uuid not null,
    constraint fk__claim__claim_section
        foreign key (claim_section_id)
        references claim_section(id),
    name citext not null,
    description citext not null,
    created_date timestamp without time zone  not null default utcnow(),
    modified_date timestamp without time zone  not null default utcnow(),
    created_by uuid null,
    modified_by uuid null,
    is_active boolean not null default true
)
with (
     fillfactor = 80
   , autovacuum_analyze_scale_factor = 0.002
   , autovacuum_vacuum_scale_factor = 0.001);

create table user_community_claim (
    user_id uuid not null,
    community_id uuid not null,
    claim_id uuid not null,
    constraint pk__user_community_claim
        primary key (community_id, user_id, claim_id),
    constraint fk__user_community_claim__user_community
        foreign key (user_id, community_id)
        references user_community(user_id, community_id),
    created_date timestamp without time zone  not null default utcnow(),
    modified_date timestamp without time zone  not null default utcnow(),
    created_by uuid null,
    modified_by uuid null,
    is_active boolean not null default true
)
with (
     fillfactor = 80
   , autovacuum_analyze_scale_factor = 0.002
   , autovacuum_vacuum_scale_factor = 0.001);

create table report_type (
    id uuid not null default uuid7(),
    constraint pk__report_type__id
        primary key (id),
    name citext not null,
    icon varchar(255) not null,
    sort_order smallint not null default 100,
    created_date timestamp without time zone not null default utcnow(),
    modified_date timestamp without time zone not null default utcnow(),
    created_by uuid null,
    modified_by uuid null,
    is_active boolean not null default true
)
with (
     fillfactor = 80
   , autovacuum_analyze_scale_factor = 0.002
   , autovacuum_vacuum_scale_factor = 0.001);

create table report (
    id uuid not null default uuid7(),
    community_id uuid not null,
    constraint fk__report__community
        foreign key (community_id)
        references community(id),
    constraint pk__report__id__community_id
        primary key(id, community_id),
    report_type_id uuid not null,
    constraint fk__report__report_type
        foreign key (report_type_id)
        references report_type(id),
    priority smallint not null,
    sticky bool not null default false,
    schedule_date timestamp without time zone null,
    end_date timestamp without time zone null,
    message citext null,
    longitude numeric(9,6) not null,
    latitude numeric(9,6) not null,
    end_longitude numeric(9,6) null,
    end_latitude numeric(9,6) null,
    resolved bool not null default false,
    resolved_date timestamp without time zone null,
    resolved_user uuid null,
    constraint fk__report__user__resolved_by
        foreign key (resolved_user)
        references "user"(id),
    created_date timestamp without time zone not null default utcnow(),
    modified_date timestamp without time zone not null default utcnow(),
    created_by uuid null,
    modified_by uuid null,
    is_active boolean not null default true
)
with (
     fillfactor = 80
   , autovacuum_analyze_scale_factor = 0.002
   , autovacuum_vacuum_scale_factor = 0.001);

create table report_user_acknowledgement (
    report_id uuid not null,
    community_id uuid not null,
    constraint fk__report_user_acknowledgement__report
        foreign key (report_id, community_id)
        references report(id, community_id),
    user_id uuid not null,
    constraint fk__report_user_acknowledgement__user
        foreign key (user_id)
        references "user"(id),
    constraint pk__report_user_acknowledgment__report__community__user
        primary key (report_id, community_id, user_id),
    created_date timestamp without time zone not null default utcnow(),
    modified_date timestamp without time zone not null default utcnow(),
    created_by uuid null,
    modified_by uuid null,
    is_active boolean not null default true
)
with (
     fillfactor = 80
   , autovacuum_analyze_scale_factor = 0.002
   , autovacuum_vacuum_scale_factor = 0.001);

create table report_type__user_community__subscription (
    report_type_id uuid not null,
    constraint fk__user_report_type_subscription__report_type
        foreign key (report_type_id)
        references report_type(id),
    user_id uuid not null,
    community_id uuid not null,
    constraint fk__report_type_user_subscription__user_community
        foreign key (user_id, community_id)
        references user_community(user_id, community_id),
    notification_type int not null,
    created_date timestamp without time zone not null default utcnow(),
    modified_date timestamp without time zone not null default utcnow(),
    created_by uuid null,
    modified_by uuid null,
    is_active boolean not null default true
)
with (
     fillfactor = 80
   , autovacuum_analyze_scale_factor = 0.002
   , autovacuum_vacuum_scale_factor = 0.001);

grant all privileges on all tables in schema public to community;