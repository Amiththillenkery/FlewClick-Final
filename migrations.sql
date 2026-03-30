CREATE TABLE IF NOT EXISTS "__EFMigrationsHistory" (
    "MigrationId" character varying(150) NOT NULL,
    "ProductVersion" character varying(32) NOT NULL,
    CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId")
);

START TRANSACTION;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260323130112_AddAppUserWithRoles') THEN
    CREATE TABLE app_users (
        id uuid NOT NULL,
        full_name character varying(150) NOT NULL,
        email character varying(254) NOT NULL,
        phone character varying(20),
        user_type character varying(30) NOT NULL,
        professional_role character varying(40),
        is_active boolean NOT NULL,
        created_at_utc timestamp with time zone NOT NULL,
        updated_at_utc timestamp with time zone,
        CONSTRAINT "PK_app_users" PRIMARY KEY (id)
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260323130112_AddAppUserWithRoles') THEN
    CREATE UNIQUE INDEX "IX_app_users_email" ON app_users (email);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260323130112_AddAppUserWithRoles') THEN
    CREATE INDEX "IX_app_users_professional_role" ON app_users (professional_role);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260323130112_AddAppUserWithRoles') THEN
    CREATE INDEX "IX_app_users_user_type" ON app_users (user_type);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260323130112_AddAppUserWithRoles') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20260323130112_AddAppUserWithRoles', '10.0.5');
    END IF;
END $EF$;
COMMIT;

START TRANSACTION;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260323132248_AddProfessionalRegistrationTables') THEN
    CREATE TABLE drone_configs (
        id uuid NOT NULL,
        professional_profile_id uuid NOT NULL,
        drone_model character varying(200) NOT NULL,
        license_number character varying(100),
        has_flight_certification boolean NOT NULL,
        max_flight_altitude_meters integer,
        capabilities jsonb NOT NULL,
        created_at_utc timestamp with time zone NOT NULL,
        updated_at_utc timestamp with time zone,
        CONSTRAINT "PK_drone_configs" PRIMARY KEY (id)
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260323132248_AddProfessionalRegistrationTables') THEN
    CREATE TABLE editing_configs (
        id uuid NOT NULL,
        professional_profile_id uuid NOT NULL,
        software_tools jsonb NOT NULL,
        specializations jsonb NOT NULL,
        output_formats character varying(500),
        created_at_utc timestamp with time zone NOT NULL,
        updated_at_utc timestamp with time zone,
        CONSTRAINT "PK_editing_configs" PRIMARY KEY (id)
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260323132248_AddProfessionalRegistrationTables') THEN
    CREATE TABLE photography_configs (
        id uuid NOT NULL,
        professional_profile_id uuid NOT NULL,
        styles jsonb NOT NULL,
        camera_gear character varying(500),
        shoot_types character varying(500),
        has_studio boolean NOT NULL,
        created_at_utc timestamp with time zone NOT NULL,
        updated_at_utc timestamp with time zone,
        CONSTRAINT "PK_photography_configs" PRIMARY KEY (id)
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260323132248_AddProfessionalRegistrationTables') THEN
    CREATE TABLE professional_profiles (
        id uuid NOT NULL,
        app_user_id uuid NOT NULL,
        bio character varying(1000),
        location character varying(200),
        years_of_experience integer,
        hourly_rate numeric(10,2),
        portfolio_url character varying(500),
        is_registration_complete boolean NOT NULL,
        created_at_utc timestamp with time zone NOT NULL,
        updated_at_utc timestamp with time zone,
        CONSTRAINT "PK_professional_profiles" PRIMARY KEY (id)
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260323132248_AddProfessionalRegistrationTables') THEN
    CREATE TABLE rental_equipments (
        id uuid NOT NULL,
        professional_profile_id uuid NOT NULL,
        equipment_name character varying(200) NOT NULL,
        equipment_type character varying(100),
        brand character varying(100),
        daily_rental_rate numeric(10,2) NOT NULL,
        is_available boolean NOT NULL,
        condition_notes character varying(500),
        created_at_utc timestamp with time zone NOT NULL,
        updated_at_utc timestamp with time zone,
        CONSTRAINT "PK_rental_equipments" PRIMARY KEY (id)
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260323132248_AddProfessionalRegistrationTables') THEN
    CREATE UNIQUE INDEX "IX_drone_configs_professional_profile_id" ON drone_configs (professional_profile_id);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260323132248_AddProfessionalRegistrationTables') THEN
    CREATE UNIQUE INDEX "IX_editing_configs_professional_profile_id" ON editing_configs (professional_profile_id);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260323132248_AddProfessionalRegistrationTables') THEN
    CREATE UNIQUE INDEX "IX_photography_configs_professional_profile_id" ON photography_configs (professional_profile_id);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260323132248_AddProfessionalRegistrationTables') THEN
    CREATE UNIQUE INDEX "IX_professional_profiles_app_user_id" ON professional_profiles (app_user_id);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260323132248_AddProfessionalRegistrationTables') THEN
    CREATE INDEX "IX_rental_equipments_professional_profile_id" ON rental_equipments (professional_profile_id);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260323132248_AddProfessionalRegistrationTables') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20260323132248_AddProfessionalRegistrationTables', '10.0.5');
    END IF;
END $EF$;
COMMIT;

START TRANSACTION;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260323140613_SplitPhotographerVideographerRoles') THEN
    DROP INDEX "IX_app_users_professional_role";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260323140613_SplitPhotographerVideographerRoles') THEN
    ALTER TABLE app_users DROP COLUMN professional_role;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260323140613_SplitPhotographerVideographerRoles') THEN
    ALTER TABLE app_users ADD professional_roles jsonb NOT NULL DEFAULT '[]';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260323140613_SplitPhotographerVideographerRoles') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20260323140613_SplitPhotographerVideographerRoles', '10.0.5');
    END IF;
END $EF$;
COMMIT;

START TRANSACTION;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260323143532_AddPackageAndRentalStoreTables') THEN
    CREATE TABLE deliverable_masters (
        id uuid NOT NULL,
        role_type character varying(30) NOT NULL,
        name character varying(200) NOT NULL,
        description character varying(500),
        category character varying(30) NOT NULL,
        configurable_attrs jsonb NOT NULL,
        is_active boolean NOT NULL,
        created_at_utc timestamp with time zone NOT NULL,
        updated_at_utc timestamp with time zone,
        CONSTRAINT "PK_deliverable_masters" PRIMARY KEY (id)
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260323143532_AddPackageAndRentalStoreTables') THEN
    CREATE TABLE package_deliverables (
        id uuid NOT NULL,
        package_id uuid NOT NULL,
        deliverable_master_id uuid NOT NULL,
        quantity integer NOT NULL,
        configuration jsonb NOT NULL,
        notes character varying(500),
        created_at_utc timestamp with time zone NOT NULL,
        updated_at_utc timestamp with time zone,
        CONSTRAINT "PK_package_deliverables" PRIMARY KEY (id)
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260323143532_AddPackageAndRentalStoreTables') THEN
    CREATE TABLE package_pricings (
        id uuid NOT NULL,
        package_id uuid NOT NULL,
        pricing_type character varying(30) NOT NULL,
        base_price numeric(12,2) NOT NULL,
        discount_percentage numeric(5,2),
        final_price numeric(12,2) NOT NULL,
        duration_hours integer,
        is_negotiable boolean NOT NULL,
        notes character varying(500),
        created_at_utc timestamp with time zone NOT NULL,
        updated_at_utc timestamp with time zone,
        CONSTRAINT "PK_package_pricings" PRIMARY KEY (id)
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260323143532_AddPackageAndRentalStoreTables') THEN
    CREATE TABLE packages (
        id uuid NOT NULL,
        professional_profile_id uuid NOT NULL,
        name character varying(200) NOT NULL,
        description character varying(2000),
        package_type character varying(30) NOT NULL,
        coverage_type character varying(30),
        is_active boolean NOT NULL,
        created_at_utc timestamp with time zone NOT NULL,
        updated_at_utc timestamp with time zone,
        CONSTRAINT "PK_packages" PRIMARY KEY (id)
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260323143532_AddPackageAndRentalStoreTables') THEN
    CREATE TABLE rental_product_images (
        id uuid NOT NULL,
        rental_product_id uuid NOT NULL,
        image_url character varying(1000) NOT NULL,
        display_order integer NOT NULL,
        is_primary boolean NOT NULL,
        created_at_utc timestamp with time zone NOT NULL,
        updated_at_utc timestamp with time zone,
        CONSTRAINT "PK_rental_product_images" PRIMARY KEY (id)
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260323143532_AddPackageAndRentalStoreTables') THEN
    CREATE TABLE rental_product_pricings (
        id uuid NOT NULL,
        rental_product_id uuid NOT NULL,
        hourly_rate numeric(10,2),
        daily_rate numeric(10,2),
        weekly_rate numeric(10,2),
        monthly_rate numeric(10,2),
        deposit_amount numeric(10,2) NOT NULL,
        created_at_utc timestamp with time zone NOT NULL,
        updated_at_utc timestamp with time zone,
        CONSTRAINT "PK_rental_product_pricings" PRIMARY KEY (id)
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260323143532_AddPackageAndRentalStoreTables') THEN
    CREATE TABLE rental_products (
        id uuid NOT NULL,
        rental_store_id uuid NOT NULL,
        name character varying(200) NOT NULL,
        description character varying(2000),
        category character varying(100),
        brand character varying(100),
        model character varying(200),
        condition character varying(20) NOT NULL,
        specifications jsonb NOT NULL,
        is_available boolean NOT NULL,
        created_at_utc timestamp with time zone NOT NULL,
        updated_at_utc timestamp with time zone,
        CONSTRAINT "PK_rental_products" PRIMARY KEY (id)
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260323143532_AddPackageAndRentalStoreTables') THEN
    CREATE TABLE rental_stores (
        id uuid NOT NULL,
        professional_profile_id uuid NOT NULL,
        store_name character varying(200) NOT NULL,
        description character varying(2000),
        policies jsonb NOT NULL,
        is_active boolean NOT NULL,
        created_at_utc timestamp with time zone NOT NULL,
        updated_at_utc timestamp with time zone,
        CONSTRAINT "PK_rental_stores" PRIMARY KEY (id)
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260323143532_AddPackageAndRentalStoreTables') THEN
    CREATE INDEX "IX_deliverable_masters_role_type" ON deliverable_masters (role_type);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260323143532_AddPackageAndRentalStoreTables') THEN
    CREATE INDEX "IX_deliverable_masters_role_type_category" ON deliverable_masters (role_type, category);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260323143532_AddPackageAndRentalStoreTables') THEN
    CREATE INDEX "IX_package_deliverables_package_id" ON package_deliverables (package_id);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260323143532_AddPackageAndRentalStoreTables') THEN
    CREATE UNIQUE INDEX "IX_package_deliverables_package_id_deliverable_master_id" ON package_deliverables (package_id, deliverable_master_id);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260323143532_AddPackageAndRentalStoreTables') THEN
    CREATE UNIQUE INDEX "IX_package_pricings_package_id" ON package_pricings (package_id);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260323143532_AddPackageAndRentalStoreTables') THEN
    CREATE INDEX "IX_packages_professional_profile_id" ON packages (professional_profile_id);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260323143532_AddPackageAndRentalStoreTables') THEN
    CREATE INDEX "IX_packages_professional_profile_id_package_type" ON packages (professional_profile_id, package_type);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260323143532_AddPackageAndRentalStoreTables') THEN
    CREATE INDEX "IX_rental_product_images_rental_product_id" ON rental_product_images (rental_product_id);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260323143532_AddPackageAndRentalStoreTables') THEN
    CREATE UNIQUE INDEX "IX_rental_product_pricings_rental_product_id" ON rental_product_pricings (rental_product_id);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260323143532_AddPackageAndRentalStoreTables') THEN
    CREATE INDEX "IX_rental_products_rental_store_id" ON rental_products (rental_store_id);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260323143532_AddPackageAndRentalStoreTables') THEN
    CREATE INDEX "IX_rental_products_rental_store_id_category" ON rental_products (rental_store_id, category);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260323143532_AddPackageAndRentalStoreTables') THEN
    CREATE UNIQUE INDEX "IX_rental_stores_professional_profile_id" ON rental_stores (professional_profile_id);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260323143532_AddPackageAndRentalStoreTables') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20260323143532_AddPackageAndRentalStoreTables', '10.0.5');
    END IF;
END $EF$;
COMMIT;

START TRANSACTION;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260325065402_AddPortfolioAndInstagram') THEN
    ALTER TABLE app_users ADD "PasswordHash" text;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260325065402_AddPortfolioAndInstagram') THEN
    CREATE TABLE instagram_connections (
        id uuid NOT NULL,
        professional_profile_id uuid NOT NULL,
        instagram_user_id character varying(100) NOT NULL,
        access_token character varying(1000) NOT NULL,
        token_expires_at timestamp with time zone NOT NULL,
        username character varying(100) NOT NULL,
        is_active boolean NOT NULL,
        last_sync_at timestamp with time zone,
        created_at_utc timestamp with time zone NOT NULL,
        updated_at_utc timestamp with time zone,
        CONSTRAINT "PK_instagram_connections" PRIMARY KEY (id)
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260325065402_AddPortfolioAndInstagram') THEN
    CREATE TABLE portfolio_items (
        id uuid NOT NULL,
        professional_profile_id uuid NOT NULL,
        instagram_media_id character varying(100) NOT NULL,
        media_type character varying(30) NOT NULL,
        media_url character varying(2000) NOT NULL,
        thumbnail_url character varying(2000),
        caption character varying(2200),
        permalink character varying(500) NOT NULL,
        posted_at timestamp with time zone NOT NULL,
        display_order integer NOT NULL,
        is_visible boolean NOT NULL,
        created_at_utc timestamp with time zone NOT NULL,
        updated_at_utc timestamp with time zone,
        CONSTRAINT "PK_portfolio_items" PRIMARY KEY (id)
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260325065402_AddPortfolioAndInstagram') THEN
    CREATE INDEX "IX_instagram_connections_instagram_user_id" ON instagram_connections (instagram_user_id);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260325065402_AddPortfolioAndInstagram') THEN
    CREATE UNIQUE INDEX "IX_instagram_connections_professional_profile_id" ON instagram_connections (professional_profile_id);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260325065402_AddPortfolioAndInstagram') THEN
    CREATE INDEX "IX_portfolio_items_professional_profile_id" ON portfolio_items (professional_profile_id);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260325065402_AddPortfolioAndInstagram') THEN
    CREATE UNIQUE INDEX "IX_portfolio_items_professional_profile_id_instagram_media_id" ON portfolio_items (professional_profile_id, instagram_media_id);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260325065402_AddPortfolioAndInstagram') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20260325065402_AddPortfolioAndInstagram', '10.0.5');
    END IF;
END $EF$;
COMMIT;

START TRANSACTION;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260325102902_AddConsumerAndRelatedEntities') THEN
    CREATE TABLE consumers (
        id uuid NOT NULL,
        phone character varying(20) NOT NULL,
        full_name character varying(150) NOT NULL,
        email character varying(254),
        is_phone_verified boolean NOT NULL,
        is_active boolean NOT NULL,
        last_login_at timestamp with time zone,
        created_at_utc timestamp with time zone NOT NULL,
        updated_at_utc timestamp with time zone,
        CONSTRAINT "PK_consumers" PRIMARY KEY (id)
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260325102902_AddConsumerAndRelatedEntities') THEN
    CREATE TABLE otp_verifications (
        id uuid NOT NULL,
        phone character varying(20) NOT NULL,
        code character varying(6) NOT NULL,
        purpose character varying(20) NOT NULL,
        expires_at timestamp with time zone NOT NULL,
        is_used boolean NOT NULL,
        created_at_utc timestamp with time zone NOT NULL,
        updated_at_utc timestamp with time zone,
        CONSTRAINT "PK_otp_verifications" PRIMARY KEY (id)
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260325102902_AddConsumerAndRelatedEntities') THEN
    CREATE TABLE saved_professionals (
        id uuid NOT NULL,
        consumer_id uuid NOT NULL,
        professional_profile_id uuid NOT NULL,
        created_at_utc timestamp with time zone NOT NULL,
        updated_at_utc timestamp with time zone,
        CONSTRAINT "PK_saved_professionals" PRIMARY KEY (id)
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260325102902_AddConsumerAndRelatedEntities') THEN
    CREATE UNIQUE INDEX "IX_consumers_phone" ON consumers (phone);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260325102902_AddConsumerAndRelatedEntities') THEN
    CREATE INDEX "IX_otp_verifications_phone_purpose" ON otp_verifications (phone, purpose);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260325102902_AddConsumerAndRelatedEntities') THEN
    CREATE INDEX "IX_saved_professionals_consumer_id" ON saved_professionals (consumer_id);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260325102902_AddConsumerAndRelatedEntities') THEN
    CREATE UNIQUE INDEX "IX_saved_professionals_consumer_id_professional_profile_id" ON saved_professionals (consumer_id, professional_profile_id);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260325102902_AddConsumerAndRelatedEntities') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20260325102902_AddConsumerAndRelatedEntities', '10.0.5');
    END IF;
END $EF$;
COMMIT;

START TRANSACTION;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260325114119_AddBookingWorkflow') THEN
    CREATE TABLE agreement_deliverables (
        id uuid NOT NULL,
        agreement_id uuid NOT NULL,
        deliverable_name character varying(200) NOT NULL,
        quantity integer NOT NULL,
        configuration jsonb NOT NULL,
        notes character varying(1000),
        created_at_utc timestamp with time zone NOT NULL,
        updated_at_utc timestamp with time zone,
        CONSTRAINT "PK_agreement_deliverables" PRIMARY KEY (id)
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260325114119_AddBookingWorkflow') THEN
    CREATE TABLE agreements (
        id uuid NOT NULL,
        booking_request_id uuid NOT NULL,
        version integer NOT NULL,
        package_snapshot jsonb NOT NULL,
        event_date timestamp with time zone NOT NULL,
        event_location character varying(500),
        event_description character varying(2000),
        total_price numeric(18,2) NOT NULL,
        terms character varying(5000),
        conditions character varying(5000),
        notes character varying(2000),
        status character varying(20) NOT NULL,
        created_at_utc timestamp with time zone NOT NULL,
        updated_at_utc timestamp with time zone,
        CONSTRAINT "PK_agreements" PRIMARY KEY (id)
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260325114119_AddBookingWorkflow') THEN
    CREATE TABLE booking_requests (
        id uuid NOT NULL,
        consumer_id uuid NOT NULL,
        professional_profile_id uuid NOT NULL,
        package_id uuid NOT NULL,
        event_date timestamp with time zone NOT NULL,
        event_location character varying(500),
        notes character varying(2000),
        status character varying(30) NOT NULL,
        decline_reason character varying(1000),
        cancellation_reason character varying(1000),
        cancelled_by character varying(20),
        created_at_utc timestamp with time zone NOT NULL,
        updated_at_utc timestamp with time zone,
        CONSTRAINT "PK_booking_requests" PRIMARY KEY (id)
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260325114119_AddBookingWorkflow') THEN
    CREATE TABLE booking_status_history (
        id uuid NOT NULL,
        booking_request_id uuid NOT NULL,
        from_status character varying(30),
        to_status character varying(30) NOT NULL,
        changed_by character varying(200) NOT NULL,
        changed_by_type character varying(20) NOT NULL,
        reason character varying(2000),
        created_at_utc timestamp with time zone NOT NULL,
        updated_at_utc timestamp with time zone,
        CONSTRAINT "PK_booking_status_history" PRIMARY KEY (id)
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260325114119_AddBookingWorkflow') THEN
    CREATE TABLE chat_messages (
        id uuid NOT NULL,
        conversation_id uuid NOT NULL,
        sender_id uuid NOT NULL,
        sender_type character varying(20) NOT NULL,
        content character varying(5000) NOT NULL,
        is_read boolean NOT NULL,
        read_at timestamp with time zone,
        created_at_utc timestamp with time zone NOT NULL,
        updated_at_utc timestamp with time zone,
        CONSTRAINT "PK_chat_messages" PRIMARY KEY (id)
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260325114119_AddBookingWorkflow') THEN
    CREATE TABLE conversations (
        id uuid NOT NULL,
        booking_request_id uuid NOT NULL,
        consumer_id uuid NOT NULL,
        professional_profile_id uuid NOT NULL,
        is_active boolean NOT NULL,
        created_at_utc timestamp with time zone NOT NULL,
        updated_at_utc timestamp with time zone,
        CONSTRAINT "PK_conversations" PRIMARY KEY (id)
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260325114119_AddBookingWorkflow') THEN
    CREATE TABLE platform_fee_payments (
        id uuid NOT NULL,
        booking_request_id uuid NOT NULL,
        professional_profile_id uuid NOT NULL,
        agreement_amount numeric(18,2) NOT NULL,
        fee_percentage numeric(5,2) NOT NULL,
        fee_amount numeric(18,2) NOT NULL,
        status character varying(20) NOT NULL,
        razorpay_order_id character varying(100),
        razorpay_payment_id character varying(100),
        razorpay_signature character varying(500),
        paid_at timestamp with time zone,
        failure_reason character varying(1000),
        due_date timestamp with time zone NOT NULL,
        created_at_utc timestamp with time zone NOT NULL,
        updated_at_utc timestamp with time zone,
        CONSTRAINT "PK_platform_fee_payments" PRIMARY KEY (id)
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260325114119_AddBookingWorkflow') THEN
    CREATE INDEX "IX_agreement_deliverables_agreement_id" ON agreement_deliverables (agreement_id);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260325114119_AddBookingWorkflow') THEN
    CREATE INDEX "IX_agreements_booking_request_id" ON agreements (booking_request_id);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260325114119_AddBookingWorkflow') THEN
    CREATE UNIQUE INDEX "IX_agreements_booking_request_id_version" ON agreements (booking_request_id, version);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260325114119_AddBookingWorkflow') THEN
    CREATE INDEX "IX_booking_requests_consumer_id" ON booking_requests (consumer_id);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260325114119_AddBookingWorkflow') THEN
    CREATE INDEX "IX_booking_requests_professional_profile_id" ON booking_requests (professional_profile_id);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260325114119_AddBookingWorkflow') THEN
    CREATE INDEX "IX_booking_requests_status" ON booking_requests (status);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260325114119_AddBookingWorkflow') THEN
    CREATE INDEX "IX_booking_status_history_booking_request_id" ON booking_status_history (booking_request_id);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260325114119_AddBookingWorkflow') THEN
    CREATE INDEX "IX_chat_messages_conversation_id_created_at_utc" ON chat_messages (conversation_id, created_at_utc);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260325114119_AddBookingWorkflow') THEN
    CREATE UNIQUE INDEX "IX_conversations_booking_request_id" ON conversations (booking_request_id);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260325114119_AddBookingWorkflow') THEN
    CREATE INDEX "IX_platform_fee_payments_booking_request_id" ON platform_fee_payments (booking_request_id);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260325114119_AddBookingWorkflow') THEN
    CREATE INDEX "IX_platform_fee_payments_professional_profile_id_status" ON platform_fee_payments (professional_profile_id, status);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260325114119_AddBookingWorkflow') THEN
    CREATE INDEX "IX_platform_fee_payments_razorpay_order_id" ON platform_fee_payments (razorpay_order_id);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260325114119_AddBookingWorkflow') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20260325114119_AddBookingWorkflow', '10.0.5');
    END IF;
END $EF$;
COMMIT;

START TRANSACTION;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260325154300_SeedDeliverableMasters') THEN
    INSERT INTO deliverable_masters (id, category, configurable_attrs, created_at_utc, description, is_active, name, role_type, updated_at_utc)
    VALUES ('a0000001-0000-0000-0000-000000000001', 'Print', '{"leaf_count":40,"size":"A4"}', TIMESTAMPTZ '2026-01-01T00:00:00Z', 'Printed photo album with customizable pages', TRUE, 'Photo Album', 'Photographer', NULL);
    INSERT INTO deliverable_masters (id, category, configurable_attrs, created_at_utc, description, is_active, name, role_type, updated_at_utc)
    VALUES ('a0000001-0000-0000-0000-000000000002', 'Print', '{"size":"16x20","frame":"black"}', TIMESTAMPTZ '2026-01-01T00:00:00Z', 'Gallery-wrapped canvas photo print', TRUE, 'Canvas Print', 'Photographer', NULL);
    INSERT INTO deliverable_masters (id, category, configurable_attrs, created_at_utc, description, is_active, name, role_type, updated_at_utc)
    VALUES ('a0000001-0000-0000-0000-000000000003', 'Print', '{"size":"8x10","frame_material":"wood"}', TIMESTAMPTZ '2026-01-01T00:00:00Z', 'Single framed photograph with mat', TRUE, 'Framed Photo', 'Photographer', NULL);
    INSERT INTO deliverable_masters (id, category, configurable_attrs, created_at_utc, description, is_active, name, role_type, updated_at_utc)
    VALUES ('a0000001-0000-0000-0000-000000000004', 'Digital', '{"resolution":"full","format":"JPEG"}', TIMESTAMPTZ '2026-01-01T00:00:00Z', 'Professionally edited high-resolution digital photos', TRUE, 'Edited Digital Photos', 'Photographer', NULL);
    INSERT INTO deliverable_masters (id, category, configurable_attrs, created_at_utc, description, is_active, name, role_type, updated_at_utc)
    VALUES ('a0000001-0000-0000-0000-000000000005', 'Digital', '{"format":"RAW\u002BJPEG"}', TIMESTAMPTZ '2026-01-01T00:00:00Z', 'All raw unedited photos from the shoot', TRUE, 'Raw Unedited Photos', 'Photographer', NULL);
    INSERT INTO deliverable_masters (id, category, configurable_attrs, created_at_utc, description, is_active, name, role_type, updated_at_utc)
    VALUES ('a0000001-0000-0000-0000-000000000006', 'Digital', '{"duration_days":90}', TIMESTAMPTZ '2026-01-01T00:00:00Z', 'Password-protected online gallery for sharing', TRUE, 'Online Gallery', 'Photographer', NULL);
    INSERT INTO deliverable_masters (id, category, configurable_attrs, created_at_utc, description, is_active, name, role_type, updated_at_utc)
    VALUES ('a0000001-0000-0000-0000-000000000007', 'Physical', '{"capacity_gb":32}', TIMESTAMPTZ '2026-01-01T00:00:00Z', 'All photos delivered on a branded USB drive', TRUE, 'USB Drive', 'Photographer', NULL);
    INSERT INTO deliverable_masters (id, category, configurable_attrs, created_at_utc, description, is_active, name, role_type, updated_at_utc)
    VALUES ('a0000001-0000-0000-0000-000000000008', 'Service', '{"duration_hours":3,"props_included":true}', TIMESTAMPTZ '2026-01-01T00:00:00Z', 'On-site photo booth with props and instant prints', TRUE, 'Photo Booth Setup', 'Photographer', NULL);
    INSERT INTO deliverable_masters (id, category, configurable_attrs, created_at_utc, description, is_active, name, role_type, updated_at_utc)
    VALUES ('a0000002-0000-0000-0000-000000000001', 'Digital', '{"duration_minutes":5,"resolution":"4K"}', TIMESTAMPTZ '2026-01-01T00:00:00Z', 'Short cinematic highlight video of the event', TRUE, 'Highlight Reel', 'Videographer', NULL);
    INSERT INTO deliverable_masters (id, category, configurable_attrs, created_at_utc, description, is_active, name, role_type, updated_at_utc)
    VALUES ('a0000002-0000-0000-0000-000000000002', 'Digital', '{"resolution":"4K"}', TIMESTAMPTZ '2026-01-01T00:00:00Z', 'Complete uncut video coverage of the event', TRUE, 'Full Event Video', 'Videographer', NULL);
    INSERT INTO deliverable_masters (id, category, configurable_attrs, created_at_utc, description, is_active, name, role_type, updated_at_utc)
    VALUES ('a0000002-0000-0000-0000-000000000003', 'Digital', '{"duration_seconds":60,"aspect_ratio":"9:16"}', TIMESTAMPTZ '2026-01-01T00:00:00Z', '30-60 second social media teaser clip', TRUE, 'Teaser Video', 'Videographer', NULL);
    INSERT INTO deliverable_masters (id, category, configurable_attrs, created_at_utc, description, is_active, name, role_type, updated_at_utc)
    VALUES ('a0000002-0000-0000-0000-000000000004', 'Digital', '{"resolution":"4K"}', TIMESTAMPTZ '2026-01-01T00:00:00Z', 'Aerial video footage captured by drone', TRUE, 'Drone Aerial Footage', 'Videographer', NULL);
    INSERT INTO deliverable_masters (id, category, configurable_attrs, created_at_utc, description, is_active, name, role_type, updated_at_utc)
    VALUES ('a0000002-0000-0000-0000-000000000005', 'Service', '{"duration_minutes":3}', TIMESTAMPTZ '2026-01-01T00:00:00Z', 'Quick-turnaround edited video delivered same day', TRUE, 'Same Day Edit', 'Videographer', NULL);
    INSERT INTO deliverable_masters (id, category, configurable_attrs, created_at_utc, description, is_active, name, role_type, updated_at_utc)
    VALUES ('a0000002-0000-0000-0000-000000000006', 'Digital', '{"format":"MP4"}', TIMESTAMPTZ '2026-01-01T00:00:00Z', 'All raw unedited video clips from the shoot', TRUE, 'Raw Video Footage', 'Videographer', NULL);
    INSERT INTO deliverable_masters (id, category, configurable_attrs, created_at_utc, description, is_active, name, role_type, updated_at_utc)
    VALUES ('a0000002-0000-0000-0000-000000000007', 'Physical', '{"format":"Blu-ray","copies":2}', TIMESTAMPTZ '2026-01-01T00:00:00Z', 'Finished video on physical disc with custom label', TRUE, 'DVD/Blu-ray', 'Videographer', NULL);
    INSERT INTO deliverable_masters (id, category, configurable_attrs, created_at_utc, description, is_active, name, role_type, updated_at_utc)
    VALUES ('a0000003-0000-0000-0000-000000000001', 'Digital', '{"level":"advanced"}', TIMESTAMPTZ '2026-01-01T00:00:00Z', 'Professional retouching per photo (skin, color, exposure)', TRUE, 'Photo Retouching', 'Editor', NULL);
    INSERT INTO deliverable_masters (id, category, configurable_attrs, created_at_utc, description, is_active, name, role_type, updated_at_utc)
    VALUES ('a0000003-0000-0000-0000-000000000002', 'Digital', '{"style":"cinematic"}', TIMESTAMPTZ '2026-01-01T00:00:00Z', 'Cinematic color grading for video footage', TRUE, 'Color Grading', 'Editor', NULL);
    INSERT INTO deliverable_masters (id, category, configurable_attrs, created_at_utc, description, is_active, name, role_type, updated_at_utc)
    VALUES ('a0000003-0000-0000-0000-000000000003', 'Digital', '{"revisions":2}', TIMESTAMPTZ '2026-01-01T00:00:00Z', 'Full video editing with transitions, music, and titles', TRUE, 'Video Editing', 'Editor', NULL);
    INSERT INTO deliverable_masters (id, category, configurable_attrs, created_at_utc, description, is_active, name, role_type, updated_at_utc)
    VALUES ('a0000003-0000-0000-0000-000000000004', 'Digital', '{"photos_count":5}', TIMESTAMPTZ '2026-01-01T00:00:00Z', 'Composite multiple photos into artistic collage', TRUE, 'Photo Compositing', 'Editor', NULL);
    INSERT INTO deliverable_masters (id, category, configurable_attrs, created_at_utc, description, is_active, name, role_type, updated_at_utc)
    VALUES ('a0000003-0000-0000-0000-000000000005', 'Digital', '{"pages":40,"style":"magazine"}', TIMESTAMPTZ '2026-01-01T00:00:00Z', 'Custom album layout design with professional typography', TRUE, 'Album Design', 'Editor', NULL);
    INSERT INTO deliverable_masters (id, category, configurable_attrs, created_at_utc, description, is_active, name, role_type, updated_at_utc)
    VALUES ('a0000003-0000-0000-0000-000000000006', 'Digital', '{}', TIMESTAMPTZ '2026-01-01T00:00:00Z', 'Professional background removal and replacement', TRUE, 'Background Removal', 'Editor', NULL);
    INSERT INTO deliverable_masters (id, category, configurable_attrs, created_at_utc, description, is_active, name, role_type, updated_at_utc)
    VALUES ('a0000004-0000-0000-0000-000000000001', 'Digital', '{"resolution":"48MP"}', TIMESTAMPTZ '2026-01-01T00:00:00Z', 'High-resolution aerial still photographs', TRUE, 'Aerial Photography', 'DroneOwner', NULL);
    INSERT INTO deliverable_masters (id, category, configurable_attrs, created_at_utc, description, is_active, name, role_type, updated_at_utc)
    VALUES ('a0000004-0000-0000-0000-000000000002', 'Digital', '{"resolution":"4K","duration_minutes":10}', TIMESTAMPTZ '2026-01-01T00:00:00Z', 'Cinematic aerial video footage', TRUE, 'Aerial Video', 'DroneOwner', NULL);
    INSERT INTO deliverable_masters (id, category, configurable_attrs, created_at_utc, description, is_active, name, role_type, updated_at_utc)
    VALUES ('a0000004-0000-0000-0000-000000000003', 'Digital', '{"format":"equirectangular"}', TIMESTAMPTZ '2026-01-01T00:00:00Z', 'Aerial 360-degree panoramic photo', TRUE, '360 Panorama', 'DroneOwner', NULL);
    INSERT INTO deliverable_masters (id, category, configurable_attrs, created_at_utc, description, is_active, name, role_type, updated_at_utc)
    VALUES ('a0000004-0000-0000-0000-000000000004', 'Service', '{"area_acres":5}', TIMESTAMPTZ '2026-01-01T00:00:00Z', 'Aerial mapping and survey flight for property', TRUE, 'Property Survey', 'DroneOwner', NULL);
    INSERT INTO deliverable_masters (id, category, configurable_attrs, created_at_utc, description, is_active, name, role_type, updated_at_utc)
    VALUES ('a0000004-0000-0000-0000-000000000005', 'Service', '{"platform":"YouTube","duration_hours":2}', TIMESTAMPTZ '2026-01-01T00:00:00Z', 'Real-time aerial live streaming of the event', TRUE, 'Live Streaming', 'DroneOwner', NULL);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260325154300_SeedDeliverableMasters') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20260325154300_SeedDeliverableMasters', '10.0.5');
    END IF;
END $EF$;
COMMIT;

START TRANSACTION;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260325162717_cascade') THEN
    UPDATE deliverable_masters SET configurable_attrs = '{"leaf_count":40,"size":"A4"}'
    WHERE id = 'a0000001-0000-0000-0000-000000000001';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260325162717_cascade') THEN
    UPDATE deliverable_masters SET configurable_attrs = '{"size":"16x20","frame":"black"}'
    WHERE id = 'a0000001-0000-0000-0000-000000000002';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260325162717_cascade') THEN
    UPDATE deliverable_masters SET configurable_attrs = '{"size":"8x10","frame_material":"wood"}'
    WHERE id = 'a0000001-0000-0000-0000-000000000003';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260325162717_cascade') THEN
    UPDATE deliverable_masters SET configurable_attrs = '{"resolution":"full","format":"JPEG"}'
    WHERE id = 'a0000001-0000-0000-0000-000000000004';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260325162717_cascade') THEN
    UPDATE deliverable_masters SET configurable_attrs = '{"format":"RAW\u002BJPEG"}'
    WHERE id = 'a0000001-0000-0000-0000-000000000005';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260325162717_cascade') THEN
    UPDATE deliverable_masters SET configurable_attrs = '{"duration_days":90}'
    WHERE id = 'a0000001-0000-0000-0000-000000000006';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260325162717_cascade') THEN
    UPDATE deliverable_masters SET configurable_attrs = '{"capacity_gb":32}'
    WHERE id = 'a0000001-0000-0000-0000-000000000007';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260325162717_cascade') THEN
    UPDATE deliverable_masters SET configurable_attrs = '{"duration_hours":3,"props_included":true}'
    WHERE id = 'a0000001-0000-0000-0000-000000000008';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260325162717_cascade') THEN
    UPDATE deliverable_masters SET configurable_attrs = '{"duration_minutes":5,"resolution":"4K"}'
    WHERE id = 'a0000002-0000-0000-0000-000000000001';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260325162717_cascade') THEN
    UPDATE deliverable_masters SET configurable_attrs = '{"resolution":"4K"}'
    WHERE id = 'a0000002-0000-0000-0000-000000000002';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260325162717_cascade') THEN
    UPDATE deliverable_masters SET configurable_attrs = '{"duration_seconds":60,"aspect_ratio":"9:16"}'
    WHERE id = 'a0000002-0000-0000-0000-000000000003';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260325162717_cascade') THEN
    UPDATE deliverable_masters SET configurable_attrs = '{"resolution":"4K"}'
    WHERE id = 'a0000002-0000-0000-0000-000000000004';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260325162717_cascade') THEN
    UPDATE deliverable_masters SET configurable_attrs = '{"duration_minutes":3}'
    WHERE id = 'a0000002-0000-0000-0000-000000000005';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260325162717_cascade') THEN
    UPDATE deliverable_masters SET configurable_attrs = '{"format":"MP4"}'
    WHERE id = 'a0000002-0000-0000-0000-000000000006';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260325162717_cascade') THEN
    UPDATE deliverable_masters SET configurable_attrs = '{"format":"Blu-ray","copies":2}'
    WHERE id = 'a0000002-0000-0000-0000-000000000007';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260325162717_cascade') THEN
    UPDATE deliverable_masters SET configurable_attrs = '{"level":"advanced"}'
    WHERE id = 'a0000003-0000-0000-0000-000000000001';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260325162717_cascade') THEN
    UPDATE deliverable_masters SET configurable_attrs = '{"style":"cinematic"}'
    WHERE id = 'a0000003-0000-0000-0000-000000000002';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260325162717_cascade') THEN
    UPDATE deliverable_masters SET configurable_attrs = '{"revisions":2}'
    WHERE id = 'a0000003-0000-0000-0000-000000000003';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260325162717_cascade') THEN
    UPDATE deliverable_masters SET configurable_attrs = '{"photos_count":5}'
    WHERE id = 'a0000003-0000-0000-0000-000000000004';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260325162717_cascade') THEN
    UPDATE deliverable_masters SET configurable_attrs = '{"pages":40,"style":"magazine"}'
    WHERE id = 'a0000003-0000-0000-0000-000000000005';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260325162717_cascade') THEN
    UPDATE deliverable_masters SET configurable_attrs = '{}'
    WHERE id = 'a0000003-0000-0000-0000-000000000006';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260325162717_cascade') THEN
    UPDATE deliverable_masters SET configurable_attrs = '{"resolution":"48MP"}'
    WHERE id = 'a0000004-0000-0000-0000-000000000001';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260325162717_cascade') THEN
    UPDATE deliverable_masters SET configurable_attrs = '{"resolution":"4K","duration_minutes":10}'
    WHERE id = 'a0000004-0000-0000-0000-000000000002';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260325162717_cascade') THEN
    UPDATE deliverable_masters SET configurable_attrs = '{"format":"equirectangular"}'
    WHERE id = 'a0000004-0000-0000-0000-000000000003';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260325162717_cascade') THEN
    UPDATE deliverable_masters SET configurable_attrs = '{"area_acres":5}'
    WHERE id = 'a0000004-0000-0000-0000-000000000004';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260325162717_cascade') THEN
    UPDATE deliverable_masters SET configurable_attrs = '{"platform":"YouTube","duration_hours":2}'
    WHERE id = 'a0000004-0000-0000-0000-000000000005';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260325162717_cascade') THEN
    ALTER TABLE package_deliverables ADD CONSTRAINT "FK_package_deliverables_packages_package_id" FOREIGN KEY (package_id) REFERENCES packages (id) ON DELETE CASCADE;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260325162717_cascade') THEN
    ALTER TABLE package_pricings ADD CONSTRAINT "FK_package_pricings_packages_package_id" FOREIGN KEY (package_id) REFERENCES packages (id) ON DELETE CASCADE;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260325162717_cascade') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20260325162717_cascade', '10.0.5');
    END IF;
END $EF$;
COMMIT;

START TRANSACTION;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260325170940_AddProfessionalAuth') THEN
    ALTER TABLE app_users RENAME COLUMN "PasswordHash" TO password_hash;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260325170940_AddProfessionalAuth') THEN
    ALTER TABLE app_users ALTER COLUMN password_hash TYPE character varying(500);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260325170940_AddProfessionalAuth') THEN
    CREATE TABLE refresh_tokens (
        id uuid NOT NULL,
        app_user_id uuid NOT NULL,
        token character varying(500) NOT NULL,
        expires_at_utc timestamp with time zone NOT NULL,
        is_revoked boolean NOT NULL,
        revoked_at_utc timestamp with time zone,
        created_by_ip character varying(50),
        replaced_by_token character varying(500),
        created_at_utc timestamp with time zone NOT NULL,
        updated_at_utc timestamp with time zone,
        CONSTRAINT "PK_refresh_tokens" PRIMARY KEY (id)
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260325170940_AddProfessionalAuth') THEN
    UPDATE deliverable_masters SET configurable_attrs = '{"leaf_count":40,"size":"A4"}'
    WHERE id = 'a0000001-0000-0000-0000-000000000001';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260325170940_AddProfessionalAuth') THEN
    UPDATE deliverable_masters SET configurable_attrs = '{"size":"16x20","frame":"black"}'
    WHERE id = 'a0000001-0000-0000-0000-000000000002';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260325170940_AddProfessionalAuth') THEN
    UPDATE deliverable_masters SET configurable_attrs = '{"size":"8x10","frame_material":"wood"}'
    WHERE id = 'a0000001-0000-0000-0000-000000000003';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260325170940_AddProfessionalAuth') THEN
    UPDATE deliverable_masters SET configurable_attrs = '{"resolution":"full","format":"JPEG"}'
    WHERE id = 'a0000001-0000-0000-0000-000000000004';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260325170940_AddProfessionalAuth') THEN
    UPDATE deliverable_masters SET configurable_attrs = '{"format":"RAW\u002BJPEG"}'
    WHERE id = 'a0000001-0000-0000-0000-000000000005';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260325170940_AddProfessionalAuth') THEN
    UPDATE deliverable_masters SET configurable_attrs = '{"duration_days":90}'
    WHERE id = 'a0000001-0000-0000-0000-000000000006';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260325170940_AddProfessionalAuth') THEN
    UPDATE deliverable_masters SET configurable_attrs = '{"capacity_gb":32}'
    WHERE id = 'a0000001-0000-0000-0000-000000000007';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260325170940_AddProfessionalAuth') THEN
    UPDATE deliverable_masters SET configurable_attrs = '{"duration_hours":3,"props_included":true}'
    WHERE id = 'a0000001-0000-0000-0000-000000000008';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260325170940_AddProfessionalAuth') THEN
    UPDATE deliverable_masters SET configurable_attrs = '{"duration_minutes":5,"resolution":"4K"}'
    WHERE id = 'a0000002-0000-0000-0000-000000000001';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260325170940_AddProfessionalAuth') THEN
    UPDATE deliverable_masters SET configurable_attrs = '{"resolution":"4K"}'
    WHERE id = 'a0000002-0000-0000-0000-000000000002';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260325170940_AddProfessionalAuth') THEN
    UPDATE deliverable_masters SET configurable_attrs = '{"duration_seconds":60,"aspect_ratio":"9:16"}'
    WHERE id = 'a0000002-0000-0000-0000-000000000003';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260325170940_AddProfessionalAuth') THEN
    UPDATE deliverable_masters SET configurable_attrs = '{"resolution":"4K"}'
    WHERE id = 'a0000002-0000-0000-0000-000000000004';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260325170940_AddProfessionalAuth') THEN
    UPDATE deliverable_masters SET configurable_attrs = '{"duration_minutes":3}'
    WHERE id = 'a0000002-0000-0000-0000-000000000005';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260325170940_AddProfessionalAuth') THEN
    UPDATE deliverable_masters SET configurable_attrs = '{"format":"MP4"}'
    WHERE id = 'a0000002-0000-0000-0000-000000000006';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260325170940_AddProfessionalAuth') THEN
    UPDATE deliverable_masters SET configurable_attrs = '{"format":"Blu-ray","copies":2}'
    WHERE id = 'a0000002-0000-0000-0000-000000000007';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260325170940_AddProfessionalAuth') THEN
    UPDATE deliverable_masters SET configurable_attrs = '{"level":"advanced"}'
    WHERE id = 'a0000003-0000-0000-0000-000000000001';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260325170940_AddProfessionalAuth') THEN
    UPDATE deliverable_masters SET configurable_attrs = '{"style":"cinematic"}'
    WHERE id = 'a0000003-0000-0000-0000-000000000002';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260325170940_AddProfessionalAuth') THEN
    UPDATE deliverable_masters SET configurable_attrs = '{"revisions":2}'
    WHERE id = 'a0000003-0000-0000-0000-000000000003';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260325170940_AddProfessionalAuth') THEN
    UPDATE deliverable_masters SET configurable_attrs = '{"photos_count":5}'
    WHERE id = 'a0000003-0000-0000-0000-000000000004';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260325170940_AddProfessionalAuth') THEN
    UPDATE deliverable_masters SET configurable_attrs = '{"pages":40,"style":"magazine"}'
    WHERE id = 'a0000003-0000-0000-0000-000000000005';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260325170940_AddProfessionalAuth') THEN
    UPDATE deliverable_masters SET configurable_attrs = '{}'
    WHERE id = 'a0000003-0000-0000-0000-000000000006';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260325170940_AddProfessionalAuth') THEN
    UPDATE deliverable_masters SET configurable_attrs = '{"resolution":"48MP"}'
    WHERE id = 'a0000004-0000-0000-0000-000000000001';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260325170940_AddProfessionalAuth') THEN
    UPDATE deliverable_masters SET configurable_attrs = '{"resolution":"4K","duration_minutes":10}'
    WHERE id = 'a0000004-0000-0000-0000-000000000002';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260325170940_AddProfessionalAuth') THEN
    UPDATE deliverable_masters SET configurable_attrs = '{"format":"equirectangular"}'
    WHERE id = 'a0000004-0000-0000-0000-000000000003';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260325170940_AddProfessionalAuth') THEN
    UPDATE deliverable_masters SET configurable_attrs = '{"area_acres":5}'
    WHERE id = 'a0000004-0000-0000-0000-000000000004';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260325170940_AddProfessionalAuth') THEN
    UPDATE deliverable_masters SET configurable_attrs = '{"platform":"YouTube","duration_hours":2}'
    WHERE id = 'a0000004-0000-0000-0000-000000000005';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260325170940_AddProfessionalAuth') THEN
    CREATE INDEX "IX_refresh_tokens_app_user_id" ON refresh_tokens (app_user_id);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260325170940_AddProfessionalAuth') THEN
    CREATE UNIQUE INDEX "IX_refresh_tokens_token" ON refresh_tokens (token);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260325170940_AddProfessionalAuth') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20260325170940_AddProfessionalAuth', '10.0.5');
    END IF;
END $EF$;
COMMIT;

START TRANSACTION;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260326155954_AddConsumerAuth') THEN
    ALTER TABLE consumers ADD password_hash character varying(500);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260326155954_AddConsumerAuth') THEN
    UPDATE deliverable_masters SET configurable_attrs = '{"leaf_count":40,"size":"A4"}'
    WHERE id = 'a0000001-0000-0000-0000-000000000001';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260326155954_AddConsumerAuth') THEN
    UPDATE deliverable_masters SET configurable_attrs = '{"size":"16x20","frame":"black"}'
    WHERE id = 'a0000001-0000-0000-0000-000000000002';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260326155954_AddConsumerAuth') THEN
    UPDATE deliverable_masters SET configurable_attrs = '{"size":"8x10","frame_material":"wood"}'
    WHERE id = 'a0000001-0000-0000-0000-000000000003';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260326155954_AddConsumerAuth') THEN
    UPDATE deliverable_masters SET configurable_attrs = '{"resolution":"full","format":"JPEG"}'
    WHERE id = 'a0000001-0000-0000-0000-000000000004';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260326155954_AddConsumerAuth') THEN
    UPDATE deliverable_masters SET configurable_attrs = '{"format":"RAW\u002BJPEG"}'
    WHERE id = 'a0000001-0000-0000-0000-000000000005';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260326155954_AddConsumerAuth') THEN
    UPDATE deliverable_masters SET configurable_attrs = '{"duration_days":90}'
    WHERE id = 'a0000001-0000-0000-0000-000000000006';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260326155954_AddConsumerAuth') THEN
    UPDATE deliverable_masters SET configurable_attrs = '{"capacity_gb":32}'
    WHERE id = 'a0000001-0000-0000-0000-000000000007';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260326155954_AddConsumerAuth') THEN
    UPDATE deliverable_masters SET configurable_attrs = '{"duration_hours":3,"props_included":true}'
    WHERE id = 'a0000001-0000-0000-0000-000000000008';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260326155954_AddConsumerAuth') THEN
    UPDATE deliverable_masters SET configurable_attrs = '{"duration_minutes":5,"resolution":"4K"}'
    WHERE id = 'a0000002-0000-0000-0000-000000000001';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260326155954_AddConsumerAuth') THEN
    UPDATE deliverable_masters SET configurable_attrs = '{"resolution":"4K"}'
    WHERE id = 'a0000002-0000-0000-0000-000000000002';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260326155954_AddConsumerAuth') THEN
    UPDATE deliverable_masters SET configurable_attrs = '{"duration_seconds":60,"aspect_ratio":"9:16"}'
    WHERE id = 'a0000002-0000-0000-0000-000000000003';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260326155954_AddConsumerAuth') THEN
    UPDATE deliverable_masters SET configurable_attrs = '{"resolution":"4K"}'
    WHERE id = 'a0000002-0000-0000-0000-000000000004';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260326155954_AddConsumerAuth') THEN
    UPDATE deliverable_masters SET configurable_attrs = '{"duration_minutes":3}'
    WHERE id = 'a0000002-0000-0000-0000-000000000005';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260326155954_AddConsumerAuth') THEN
    UPDATE deliverable_masters SET configurable_attrs = '{"format":"MP4"}'
    WHERE id = 'a0000002-0000-0000-0000-000000000006';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260326155954_AddConsumerAuth') THEN
    UPDATE deliverable_masters SET configurable_attrs = '{"format":"Blu-ray","copies":2}'
    WHERE id = 'a0000002-0000-0000-0000-000000000007';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260326155954_AddConsumerAuth') THEN
    UPDATE deliverable_masters SET configurable_attrs = '{"level":"advanced"}'
    WHERE id = 'a0000003-0000-0000-0000-000000000001';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260326155954_AddConsumerAuth') THEN
    UPDATE deliverable_masters SET configurable_attrs = '{"style":"cinematic"}'
    WHERE id = 'a0000003-0000-0000-0000-000000000002';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260326155954_AddConsumerAuth') THEN
    UPDATE deliverable_masters SET configurable_attrs = '{"revisions":2}'
    WHERE id = 'a0000003-0000-0000-0000-000000000003';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260326155954_AddConsumerAuth') THEN
    UPDATE deliverable_masters SET configurable_attrs = '{"photos_count":5}'
    WHERE id = 'a0000003-0000-0000-0000-000000000004';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260326155954_AddConsumerAuth') THEN
    UPDATE deliverable_masters SET configurable_attrs = '{"pages":40,"style":"magazine"}'
    WHERE id = 'a0000003-0000-0000-0000-000000000005';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260326155954_AddConsumerAuth') THEN
    UPDATE deliverable_masters SET configurable_attrs = '{}'
    WHERE id = 'a0000003-0000-0000-0000-000000000006';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260326155954_AddConsumerAuth') THEN
    UPDATE deliverable_masters SET configurable_attrs = '{"resolution":"48MP"}'
    WHERE id = 'a0000004-0000-0000-0000-000000000001';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260326155954_AddConsumerAuth') THEN
    UPDATE deliverable_masters SET configurable_attrs = '{"resolution":"4K","duration_minutes":10}'
    WHERE id = 'a0000004-0000-0000-0000-000000000002';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260326155954_AddConsumerAuth') THEN
    UPDATE deliverable_masters SET configurable_attrs = '{"format":"equirectangular"}'
    WHERE id = 'a0000004-0000-0000-0000-000000000003';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260326155954_AddConsumerAuth') THEN
    UPDATE deliverable_masters SET configurable_attrs = '{"area_acres":5}'
    WHERE id = 'a0000004-0000-0000-0000-000000000004';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260326155954_AddConsumerAuth') THEN
    UPDATE deliverable_masters SET configurable_attrs = '{"platform":"YouTube","duration_hours":2}'
    WHERE id = 'a0000004-0000-0000-0000-000000000005';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20260326155954_AddConsumerAuth') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20260326155954_AddConsumerAuth', '10.0.5');
    END IF;
END $EF$;
COMMIT;

