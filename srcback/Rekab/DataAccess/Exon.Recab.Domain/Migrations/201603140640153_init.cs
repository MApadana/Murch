namespace Exon.Recab.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AlertFeatureValues",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        AlertProductId = c.Long(nullable: false),
                        CategoryFeatureId = c.Long(nullable: false),
                        CustomValue = c.String(maxLength: 300),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AlertProducts", t => t.AlertProductId, cascadeDelete: false)
                .ForeignKey("dbo.CategoryFeatures", t => t.CategoryFeatureId, cascadeDelete: false)
                .Index(t => t.AlertProductId)
                .Index(t => t.CategoryFeatureId);
            
            CreateTable(
                "dbo.AlertProducts",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        UserId = c.Long(nullable: false),
                        CategoryId = c.Long(nullable: false),
                        Title = c.String(nullable: false, maxLength: 100),
                        Status = c.Int(nullable: false),
                        InsertDate = c.DateTime(nullable: false),
                        ExpireDate = c.DateTime(nullable: false),
                        SendEmail = c.Boolean(nullable: false),
                        SendSMS = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Categories", t => t.CategoryId, cascadeDelete: false)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: false)
                .Index(t => t.UserId)
                .Index(t => t.CategoryId);
            
            CreateTable(
                "dbo.Categories",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Title = c.String(nullable: false),
                        ParentId = c.Long(),
                        TodayPriceChartLastRange = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.CategoryExchanges",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        ExchangeCategoryId = c.Long(nullable: false),
                        Category_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Categories", t => t.Category_Id)
                .Index(t => t.Category_Id);
            
            CreateTable(
                "dbo.CategoryFeatures",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        CategoryId = c.Long(nullable: false),
                        ElementId = c.Long(),
                        AndroidElementId = c.Long(),
                        CategoryFeatureRangeId = c.Long(),
                        ContainerName = c.String(maxLength: 300),
                        ShowContainer = c.String(maxLength: 3000),
                        HideContainer = c.String(maxLength: 3000),
                        Title = c.String(nullable: false, maxLength: 300),
                        Pattern = c.String(maxLength: 150),
                        OrderId = c.Int(nullable: false),
                        AvailableInADS = c.Boolean(nullable: false),
                        AvailableInSearchBox = c.Boolean(nullable: false),
                        AvailableInLigthSearch = c.Boolean(nullable: false),
                        AvailableInSearchResult = c.Boolean(nullable: false),
                        LoadInFirstTime = c.Boolean(nullable: false),
                        RequiredInADInsert = c.Boolean(nullable: false),
                        AvailableInExchange = c.Boolean(nullable: false),
                        AvailableInTitle = c.Boolean(nullable: false),
                        AvailableSearchMultiSelect = c.Boolean(nullable: false),
                        AvailableInADCompaire = c.Boolean(nullable: false),
                        AvailableInADTextSearch = c.Boolean(nullable: false),
                        AvailableInRelativeADS = c.Boolean(nullable: false),
                        RelativeADSOrder = c.Int(nullable: false),
                        AvailableADSAlert = c.Boolean(nullable: false),
                        RequiredInADSAlertInsert = c.Boolean(nullable: false),
                        AvailableInReview = c.Boolean(nullable: false),
                        AvailableInRVSearch = c.Boolean(nullable: false),
                        AvailableInRVTitle = c.Boolean(nullable: false),
                        RequiredInRVInsert = c.Boolean(nullable: false),
                        AvailableInRVTextSearch = c.Boolean(nullable: false),
                        AvailableIcon = c.Boolean(nullable: false),
                        AvailableFVIcon = c.Boolean(nullable: false),
                        AvailableInArticle = c.Boolean(nullable: false),
                        RequiredInATInsert = c.Boolean(nullable: false),
                        AvailableInATSearch = c.Boolean(nullable: false),
                        AvailableInATTitle = c.Boolean(nullable: false),
                        AvailableInATTextSearch = c.Boolean(nullable: false),
                        AvailableInFeedback = c.Boolean(nullable: false),
                        AvailableTodayPrice = c.Boolean(nullable: false),
                        AvailableTPInSearch = c.Boolean(nullable: false),
                        HasCustomValue = c.Boolean(nullable: false),
                        HasMultiSelectValue = c.Boolean(nullable: false),
                        TitleOrder = c.Int(nullable: false),
                        IsMap = c.Boolean(nullable: false),
                        IsRenge = c.Boolean(nullable: false),
                        AvailableValueSearch = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AndroidElements", t => t.AndroidElementId)
                .ForeignKey("dbo.Categories", t => t.CategoryId, cascadeDelete: false)
                .ForeignKey("dbo.Elements", t => t.ElementId)
                .Index(t => t.CategoryId)
                .Index(t => t.ElementId)
                .Index(t => t.AndroidElementId);
            
            CreateTable(
                "dbo.AndroidElements",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Type = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.CategoryFeatureDependencies",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        CategoryFeatureId = c.Long(nullable: false),
                        CategoryFeatureParentId = c.Long(),
                        CategoryFeatureChildId = c.Long(),
                        CategoryFeatureHideId = c.Long(),
                        CategoryFeatureShowId = c.Long(),
                        CategoryFeature_Id = c.Long(),
                        CategoryFeature_Id1 = c.Long(),
                        CategoryFeature_Id2 = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CategoryFeatures", t => t.CategoryFeatureId, cascadeDelete: false)
                .ForeignKey("dbo.CategoryFeatures", t => t.CategoryFeatureChildId)
                .ForeignKey("dbo.CategoryFeatures", t => t.CategoryFeatureHideId)
                .ForeignKey("dbo.CategoryFeatures", t => t.CategoryFeatureParentId)
                .ForeignKey("dbo.CategoryFeatures", t => t.CategoryFeatureShowId)
                .ForeignKey("dbo.CategoryFeatures", t => t.CategoryFeature_Id)
                .ForeignKey("dbo.CategoryFeatures", t => t.CategoryFeature_Id1)
                .ForeignKey("dbo.CategoryFeatures", t => t.CategoryFeature_Id2)
                .Index(t => t.CategoryFeatureId)
                .Index(t => t.CategoryFeatureParentId)
                .Index(t => t.CategoryFeatureChildId)
                .Index(t => t.CategoryFeatureHideId)
                .Index(t => t.CategoryFeatureShowId)
                .Index(t => t.CategoryFeature_Id)
                .Index(t => t.CategoryFeature_Id1)
                .Index(t => t.CategoryFeature_Id2);
            
            CreateTable(
                "dbo.Elements",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 15),
                        HtmlId = c.String(maxLength: 15),
                        HtmlName = c.String(maxLength: 15),
                        DefaulteClass = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.FeatureValues",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 3000),
                        Description = c.String(maxLength: 3000),
                        CategoryFeatureId = c.Long(nullable: false),
                        ShowContainer = c.String(maxLength: 3000),
                        HideContainer = c.String(maxLength: 3000),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CategoryFeatures", t => t.CategoryFeatureId, cascadeDelete: false)
                .Index(t => t.CategoryFeatureId);
            
            CreateTable(
                "dbo.FeatureValueDependencies",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        FeatureValueId = c.Long(nullable: false),
                        FeatureValueParentId = c.Long(),
                        FeatureValueChildId = c.Long(),
                        CategoryFeatureHideId = c.Long(),
                        CategoryFeatureShowId = c.Long(),
                        FeatureValue_Id = c.Long(),
                        FeatureValue_Id1 = c.Long(),
                        FeatureValue_Id2 = c.Long(),
                        FeatureValue_Id3 = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CategoryFeatures", t => t.CategoryFeatureHideId)
                .ForeignKey("dbo.CategoryFeatures", t => t.CategoryFeatureShowId)
                .ForeignKey("dbo.FeatureValues", t => t.FeatureValueId, cascadeDelete: false)
                .ForeignKey("dbo.FeatureValues", t => t.FeatureValueChildId)
                .ForeignKey("dbo.FeatureValues", t => t.FeatureValueParentId)
                .ForeignKey("dbo.FeatureValues", t => t.FeatureValue_Id)
                .ForeignKey("dbo.FeatureValues", t => t.FeatureValue_Id1)
                .ForeignKey("dbo.FeatureValues", t => t.FeatureValue_Id2)
                .ForeignKey("dbo.FeatureValues", t => t.FeatureValue_Id3)
                .Index(t => t.FeatureValueId)
                .Index(t => t.FeatureValueParentId)
                .Index(t => t.FeatureValueChildId)
                .Index(t => t.CategoryFeatureHideId)
                .Index(t => t.CategoryFeatureShowId)
                .Index(t => t.FeatureValue_Id)
                .Index(t => t.FeatureValue_Id1)
                .Index(t => t.FeatureValue_Id2)
                .Index(t => t.FeatureValue_Id3);
            
            CreateTable(
                "dbo.CategoryPurchaseTypes",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        CategoryId = c.Long(nullable: false),
                        PurchaseTypeId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Categories", t => t.CategoryId, cascadeDelete: false)
                .ForeignKey("dbo.PurchaseTypes", t => t.PurchaseTypeId, cascadeDelete: false)
                .Index(t => t.CategoryId)
                .Index(t => t.PurchaseTypeId);
            
            CreateTable(
                "dbo.CategoryPurchasePackageTypes",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        CategoryPurchaseTypeId = c.Long(nullable: false),
                        PackageTypeId = c.Long(nullable: false),
                        OrderId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CategoryPurchaseTypes", t => t.CategoryPurchaseTypeId, cascadeDelete: false)
                .ForeignKey("dbo.PackageTypes", t => t.PackageTypeId, cascadeDelete: false)
                .Index(t => t.CategoryPurchaseTypeId)
                .Index(t => t.PackageTypeId);
            
            CreateTable(
                "dbo.PackageTypes",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Title = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.PurchaseConfigs",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        PackageBaseConfigId = c.Long(nullable: false),
                        CategoryPurchasePackageTypeId = c.Long(nullable: false),
                        Value = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CategoryPurchasePackageTypes", t => t.CategoryPurchasePackageTypeId, cascadeDelete: false)
                .ForeignKey("dbo.PackageBaseConfigs", t => t.PackageBaseConfigId, cascadeDelete: false)
                .Index(t => t.PackageBaseConfigId)
                .Index(t => t.CategoryPurchasePackageTypeId);
            
            CreateTable(
                "dbo.PackageBaseConfigs",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Title = c.String(),
                        ValueType = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.PackageBaseConfigValues",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Value = c.String(nullable: false, maxLength: 100),
                        PackageBaseConfigId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.PackageBaseConfigs", t => t.PackageBaseConfigId, cascadeDelete: false)
                .Index(t => t.PackageBaseConfigId);
            
            CreateTable(
                "dbo.PurchaseTypes",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 100),
                        AvailableDealership = c.Boolean(nullable: false),
                        IsFree = c.Boolean(nullable: false),
                        LogoUrl = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Email = c.String(nullable: false, maxLength: 50),
                        Password = c.String(nullable: false, maxLength: 40),
                        Status = c.Int(nullable: false),
                        FirstName = c.String(nullable: false, maxLength: 60),
                        LastName = c.String(nullable: false, maxLength: 60),
                        Mobile = c.String(nullable: false),
                        GenderType = c.Int(nullable: false),
                        LastLoginRequest = c.DateTime(nullable: false),
                        LastSuccessLogin = c.DateTime(nullable: false),
                        UnsuccessTryCount = c.Short(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Credits",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        UserId = c.Long(nullable: false),
                        TransactionId = c.Long(),
                        ParentCreditId = c.Long(),
                        VoucherId = c.Long(),
                        UserPackageCreditId = c.Long(),
                        InsertTime = c.DateTime(nullable: false),
                        Amount = c.Long(nullable: false),
                        Description = c.String(maxLength: 200),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Credits", t => t.ParentCreditId)
                .ForeignKey("dbo.Transactions", t => t.TransactionId)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: false)
                .ForeignKey("dbo.UserPackageCredits", t => t.UserPackageCreditId)
                .Index(t => t.UserId)
                .Index(t => t.TransactionId)
                .Index(t => t.ParentCreditId)
                .Index(t => t.UserPackageCreditId);
            
            CreateTable(
                "dbo.Transactions",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        UserId = c.Long(nullable: false),
                        StartTime = c.DateTime(nullable: false),
                        EndTime = c.DateTime(nullable: false),
                        Status = c.Int(nullable: false),
                        RfId = c.String(maxLength: 50),
                        BankType = c.Int(nullable: false),
                        BankResponse = c.String(maxLength: 50),
                        Amount = c.Long(nullable: false),
                        Description = c.String(maxLength: 200),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: false)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.UserPackageCredits",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        UserId = c.Long(nullable: false),
                        CategoryPurchasePackageTypeId = c.Long(nullable: false),
                        InsertTime = c.DateTime(nullable: false),
                        ExpireDate = c.DateTime(nullable: false),
                        BaseQuota = c.Long(nullable: false),
                        UsedQuota = c.Long(nullable: false),
                        Status = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CategoryPurchasePackageTypes", t => t.CategoryPurchasePackageTypeId, cascadeDelete: false)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: false)
                .Index(t => t.UserId)
                .Index(t => t.CategoryPurchasePackageTypeId);
            
            CreateTable(
                "dbo.Products",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        UserId = c.Long(nullable: false),
                        DealershipId = c.Long(),
                        CategoryId = c.Long(nullable: false),
                        UserPackageCreditId = c.Long(),
                        Title = c.String(nullable: false, maxLength: 100),
                        Description = c.String(maxLength: 1000),
                        Status = c.Int(nullable: false),
                        InsertDate = c.DateTime(nullable: false),
                        ConfirmDate = c.DateTime(),
                        ExpireDate = c.DateTime(),
                        UpdateCount = c.Int(nullable: false),
                        Priority = c.Int(nullable: false),
                        RaiseDate = c.DateTime(nullable: false),
                        RaiseBaseQuota = c.Int(nullable: false),
                        RaiseUsedQuota = c.Int(nullable: false),
                        RaiseHourTime = c.Int(nullable: false),
                        WebVisitCount = c.Long(nullable: false),
                        AndroidVisitCount = c.Long(nullable: false),
                        IosVisitCount = c.Long(nullable: false),
                        Tell = c.String(maxLength: 100),
                        CategoryExchangeId = c.Long(),
                        AdminComment = c.String(maxLength: 500),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Categories", t => t.CategoryId, cascadeDelete: false)
                .ForeignKey("dbo.Dealerships", t => t.DealershipId)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: false)
                .ForeignKey("dbo.UserPackageCredits", t => t.UserPackageCreditId)
                .Index(t => t.UserId)
                .Index(t => t.DealershipId)
                .Index(t => t.CategoryId)
                .Index(t => t.UserPackageCreditId);
            
            CreateTable(
                "dbo.Dealerships",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        UserId = c.Long(nullable: false),
                        Title = c.String(nullable: false, maxLength: 100),
                        Status = c.Int(nullable: false),
                        Address = c.String(nullable: false, maxLength: 400),
                        Tell = c.String(nullable: false, maxLength: 50),
                        Fax = c.String(maxLength: 20),
                        CoordinateLat = c.Double(nullable: false),
                        CoordinateLong = c.Double(nullable: false),
                        Description = c.String(maxLength: 300),
                        WebsiteUrl = c.String(maxLength: 200),
                        LogoUrl = c.String(maxLength: 500),
                        InsertDate = c.DateTime(nullable: false),
                        ConfirmDate = c.DateTime(),
                        CityId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Cities", t => t.CityId, cascadeDelete: false)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: false)
                .Index(t => t.UserId)
                .Index(t => t.CityId);
            
            CreateTable(
                "dbo.Cities",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        StateId = c.Long(nullable: false),
                        Name = c.String(nullable: false, maxLength: 50),
                        Latitude = c.Double(nullable: false),
                        Longitude = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.States", t => t.StateId, cascadeDelete: false)
                .Index(t => t.StateId);
            
            CreateTable(
                "dbo.States",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DealershipCategories",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        DealershipId = c.Long(nullable: false),
                        CategoryId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Categories", t => t.CategoryId, cascadeDelete: false)
                .ForeignKey("dbo.Dealerships", t => t.DealershipId, cascadeDelete: false)
                .Index(t => t.DealershipId)
                .Index(t => t.CategoryId);
            
            CreateTable(
                "dbo.ExchangeFeatureValues",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        CategoryFeatureId = c.Long(nullable: false),
                        FeatureValueId = c.Long(),
                        CustomValue = c.String(maxLength: 100),
                        Product_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CategoryFeatures", t => t.CategoryFeatureId, cascadeDelete: false)
                .ForeignKey("dbo.FeatureValues", t => t.FeatureValueId)
                .ForeignKey("dbo.Products", t => t.Product_Id)
                .Index(t => t.CategoryFeatureId)
                .Index(t => t.FeatureValueId)
                .Index(t => t.Product_Id);
            
            CreateTable(
                "dbo.ProductFeatureValues",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        ProductId = c.Long(nullable: false),
                        CategoryFeatureId = c.Long(nullable: false),
                        CustomValue = c.String(maxLength: 300),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CategoryFeatures", t => t.CategoryFeatureId, cascadeDelete: false)
                .ForeignKey("dbo.Products", t => t.ProductId, cascadeDelete: false)
                .Index(t => t.ProductId)
                .Index(t => t.CategoryFeatureId);
            
            CreateTable(
                "dbo.ProductFeatureValueFeatureValueItems",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        ProductFeatureValueId = c.Long(nullable: false),
                        FeatureValueId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.FeatureValues", t => t.FeatureValueId, cascadeDelete: false)
                .ForeignKey("dbo.ProductFeatureValues", t => t.ProductFeatureValueId, cascadeDelete: false)
                .Index(t => t.ProductFeatureValueId)
                .Index(t => t.FeatureValueId);
            
            CreateTable(
                "dbo.UserRoles",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        UserId = c.Long(nullable: false),
                        RoleId = c.Long(nullable: false),
                        Status = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Roles", t => t.RoleId, cascadeDelete: false)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: false)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.Roles",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 30),
                        Status = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Permissions",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        RoleId = c.Long(nullable: false),
                        ResourceId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Resources", t => t.ResourceId, cascadeDelete: false)
                .ForeignKey("dbo.Roles", t => t.RoleId, cascadeDelete: false)
                .Index(t => t.RoleId)
                .Index(t => t.ResourceId);
            
            CreateTable(
                "dbo.Resources",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 100),
                        Url = c.String(nullable: false, maxLength: 100),
                        Type = c.Int(nullable: false),
                        ParentId = c.Long(),
                        Key = c.String(nullable: false, maxLength: 15),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.UserTokens",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        UserId = c.Long(nullable: false),
                        Token = c.String(maxLength: 25),
                        TokenType = c.Int(nullable: false),
                        InsertTime = c.DateTime(nullable: false),
                        LastUsedTime = c.DateTime(nullable: false),
                        ClientType = c.Int(nullable: false),
                        Available = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: false)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AlertFeatureValueFeatureValueItems",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        AlertFeatureValueId = c.Long(nullable: false),
                        FeatureValueId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AlertFeatureValues", t => t.AlertFeatureValueId, cascadeDelete: false)
                .ForeignKey("dbo.FeatureValues", t => t.FeatureValueId, cascadeDelete: false)
                .Index(t => t.AlertFeatureValueId)
                .Index(t => t.FeatureValueId);
            
            CreateTable(
                "dbo.Articles",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        UserId = c.Long(nullable: false),
                        ArticleStructureId = c.Long(nullable: false),
                        Title = c.String(maxLength: 100),
                        LogoUrl = c.String(maxLength: 200),
                        BrifDescription = c.String(maxLength: 500),
                        Body = c.String(nullable: false),
                        CreateTime = c.DateTime(nullable: false),
                        Rate = c.Double(nullable: false),
                        VisitCount = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ArticleStructures", t => t.ArticleStructureId, cascadeDelete: false)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: false)
                .Index(t => t.UserId)
                .Index(t => t.ArticleStructureId);
            
            CreateTable(
                "dbo.ArticleStructures",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 100),
                        CategoryId = c.Long(nullable: false),
                        ParentArticleStructureId = c.Long(),
                        LogoUrl = c.String(maxLength: 200),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Categories", t => t.CategoryId, cascadeDelete: false)
                .ForeignKey("dbo.ArticleStructures", t => t.ParentArticleStructureId)
                .Index(t => t.CategoryId)
                .Index(t => t.ParentArticleStructureId);
            
            CreateTable(
                "dbo.ExceptionCodes",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        ExceptionType = c.Int(nullable: false),
                        Message = c.String(maxLength: 400),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.ExceptionType, unique: true);
            
            CreateTable(
                "dbo.FavouriteProducts",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        UserId = c.Long(nullable: false),
                        ProductId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Products", t => t.ProductId, cascadeDelete: false)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: false)
                .Index(t => t.UserId)
                .Index(t => t.ProductId);
            
            CreateTable(
                "dbo.FeatureValueAssigns",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        EntityId = c.Long(nullable: false),
                        EntityType = c.Int(nullable: false),
                        CategoryFeatureId = c.Long(nullable: false),
                        CustomValue = c.String(maxLength: 100),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CategoryFeatures", t => t.CategoryFeatureId, cascadeDelete: false)
                .Index(t => t.CategoryFeatureId);
            
            CreateTable(
                "dbo.FeatureValueAssignFeatureValueItems",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        FeatureValueAssignId = c.Long(nullable: false),
                        FeatureValueId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.FeatureValues", t => t.FeatureValueId, cascadeDelete: false)
                .ForeignKey("dbo.FeatureValueAssigns", t => t.FeatureValueAssignId, cascadeDelete: false)
                .Index(t => t.FeatureValueAssignId)
                .Index(t => t.FeatureValueId);
            
            CreateTable(
                "dbo.FeedbackProducts",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        UserComment = c.String(maxLength: 500),
                        UserId = c.Long(nullable: false),
                        CategoryFeatureTitle = c.String(),
                        ProductId = c.Long(nullable: false),
                        Status = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Products", t => t.ProductId, cascadeDelete: false)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: false)
                .Index(t => t.UserId)
                .Index(t => t.ProductId);
            
            CreateTable(
                "dbo.Media",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        MediaURL = c.String(nullable: false),
                        EntityId = c.Long(nullable: false),
                        Order = c.Int(nullable: false),
                        MediaType = c.Int(nullable: false),
                        EntityType = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.MongoBackUps",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Query = c.String(nullable: false),
                        BackUpUrl = c.String(nullable: false),
                        BackUpDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.RecabSystemConfigs",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        AutomaticConfirmCreatedAD = c.Boolean(nullable: false),
                        AutomaticConfirmUpdateAD = c.Boolean(nullable: false),
                        ADSPictureInFirstTime = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Reviews",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        UserId = c.Long(nullable: false),
                        CategoryId = c.Long(nullable: false),
                        Title = c.String(nullable: false, maxLength: 100),
                        Body = c.String(maxLength: 1000),
                        CreateTime = c.DateTime(nullable: false),
                        Rate = c.Double(nullable: false),
                        VisitCount = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Categories", t => t.CategoryId, cascadeDelete: false)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: false)
                .Index(t => t.UserId)
                .Index(t => t.CategoryId);
            
            CreateTable(
                "dbo.SMS",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        MobileNumber = c.String(maxLength: 12),
                        Content = c.String(maxLength: 250),
                        SendDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TodayPriceOptions",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 200),
                        CategoryId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Categories", t => t.CategoryId, cascadeDelete: false)
                .Index(t => t.CategoryId);
            
            CreateTable(
                "dbo.TodayPriceConfigs",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        UserId = c.Long(nullable: false),
                        CategoryId = c.Long(nullable: false),
                        Title = c.String(nullable: false, maxLength: 100),
                        SellOption = c.String(nullable: false, maxLength: 500),
                        Price = c.Long(nullable: false),
                        DealershipPrice = c.Long(nullable: false),
                        Tolerance = c.Double(nullable: false),
                        LastUpdateDate = c.DateTime(nullable: false),
                        CreateTime = c.DateTime(nullable: false),
                        VisitCount = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Categories", t => t.CategoryId, cascadeDelete: false)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: false)
                .Index(t => t.UserId)
                .Index(t => t.CategoryId);
            
            CreateTable(
                "dbo.Vouchers",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Code = c.String(maxLength: 20),
                        ResponseCode = c.String(maxLength: 40),
                        FromDate = c.DateTime(nullable: false),
                        ToDate = c.DateTime(nullable: false),
                        CreditId = c.Long(),
                        VoucherConfigId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Credits", t => t.CreditId)
                .ForeignKey("dbo.VoucherConfigs", t => t.VoucherConfigId, cascadeDelete: false)
                .Index(t => t.Code, unique: true)
                .Index(t => t.CreditId)
                .Index(t => t.VoucherConfigId);
            
            CreateTable(
                "dbo.VoucherConfigs",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 200),
                        Value = c.Long(nullable: false),
                        Status = c.Int(nullable: false),
                        FromDate = c.DateTime(nullable: false),
                        ToDate = c.DateTime(nullable: false),
                        CreatDate = c.DateTime(nullable: false),
                        SourceFileName = c.String(maxLength: 200),
                        Description = c.String(maxLength: 300),
                        Count = c.Long(nullable: false),
                        CreateStatus = c.Int(nullable: false),
                        UserId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: false)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Vouchers", "VoucherConfigId", "dbo.VoucherConfigs");
            DropForeignKey("dbo.VoucherConfigs", "UserId", "dbo.Users");
            DropForeignKey("dbo.Vouchers", "CreditId", "dbo.Credits");
            DropForeignKey("dbo.TodayPriceConfigs", "UserId", "dbo.Users");
            DropForeignKey("dbo.TodayPriceConfigs", "CategoryId", "dbo.Categories");
            DropForeignKey("dbo.TodayPriceOptions", "CategoryId", "dbo.Categories");
            DropForeignKey("dbo.Reviews", "UserId", "dbo.Users");
            DropForeignKey("dbo.Reviews", "CategoryId", "dbo.Categories");
            DropForeignKey("dbo.FeedbackProducts", "UserId", "dbo.Users");
            DropForeignKey("dbo.FeedbackProducts", "ProductId", "dbo.Products");
            DropForeignKey("dbo.FeatureValueAssignFeatureValueItems", "FeatureValueAssignId", "dbo.FeatureValueAssigns");
            DropForeignKey("dbo.FeatureValueAssignFeatureValueItems", "FeatureValueId", "dbo.FeatureValues");
            DropForeignKey("dbo.FeatureValueAssigns", "CategoryFeatureId", "dbo.CategoryFeatures");
            DropForeignKey("dbo.FavouriteProducts", "UserId", "dbo.Users");
            DropForeignKey("dbo.FavouriteProducts", "ProductId", "dbo.Products");
            DropForeignKey("dbo.Articles", "UserId", "dbo.Users");
            DropForeignKey("dbo.ArticleStructures", "ParentArticleStructureId", "dbo.ArticleStructures");
            DropForeignKey("dbo.ArticleStructures", "CategoryId", "dbo.Categories");
            DropForeignKey("dbo.Articles", "ArticleStructureId", "dbo.ArticleStructures");
            DropForeignKey("dbo.AlertFeatureValueFeatureValueItems", "FeatureValueId", "dbo.FeatureValues");
            DropForeignKey("dbo.AlertFeatureValueFeatureValueItems", "AlertFeatureValueId", "dbo.AlertFeatureValues");
            DropForeignKey("dbo.AlertFeatureValues", "CategoryFeatureId", "dbo.CategoryFeatures");
            DropForeignKey("dbo.AlertProducts", "UserId", "dbo.Users");
            DropForeignKey("dbo.UserTokens", "UserId", "dbo.Users");
            DropForeignKey("dbo.UserRoles", "UserId", "dbo.Users");
            DropForeignKey("dbo.UserRoles", "RoleId", "dbo.Roles");
            DropForeignKey("dbo.Permissions", "RoleId", "dbo.Roles");
            DropForeignKey("dbo.Permissions", "ResourceId", "dbo.Resources");
            DropForeignKey("dbo.Credits", "UserPackageCreditId", "dbo.UserPackageCredits");
            DropForeignKey("dbo.UserPackageCredits", "UserId", "dbo.Users");
            DropForeignKey("dbo.Products", "UserPackageCreditId", "dbo.UserPackageCredits");
            DropForeignKey("dbo.Products", "UserId", "dbo.Users");
            DropForeignKey("dbo.ProductFeatureValues", "ProductId", "dbo.Products");
            DropForeignKey("dbo.ProductFeatureValueFeatureValueItems", "ProductFeatureValueId", "dbo.ProductFeatureValues");
            DropForeignKey("dbo.ProductFeatureValueFeatureValueItems", "FeatureValueId", "dbo.FeatureValues");
            DropForeignKey("dbo.ProductFeatureValues", "CategoryFeatureId", "dbo.CategoryFeatures");
            DropForeignKey("dbo.ExchangeFeatureValues", "Product_Id", "dbo.Products");
            DropForeignKey("dbo.ExchangeFeatureValues", "FeatureValueId", "dbo.FeatureValues");
            DropForeignKey("dbo.ExchangeFeatureValues", "CategoryFeatureId", "dbo.CategoryFeatures");
            DropForeignKey("dbo.Dealerships", "UserId", "dbo.Users");
            DropForeignKey("dbo.Products", "DealershipId", "dbo.Dealerships");
            DropForeignKey("dbo.DealershipCategories", "DealershipId", "dbo.Dealerships");
            DropForeignKey("dbo.DealershipCategories", "CategoryId", "dbo.Categories");
            DropForeignKey("dbo.Dealerships", "CityId", "dbo.Cities");
            DropForeignKey("dbo.Cities", "StateId", "dbo.States");
            DropForeignKey("dbo.Products", "CategoryId", "dbo.Categories");
            DropForeignKey("dbo.UserPackageCredits", "CategoryPurchasePackageTypeId", "dbo.CategoryPurchasePackageTypes");
            DropForeignKey("dbo.Credits", "UserId", "dbo.Users");
            DropForeignKey("dbo.Credits", "TransactionId", "dbo.Transactions");
            DropForeignKey("dbo.Transactions", "UserId", "dbo.Users");
            DropForeignKey("dbo.Credits", "ParentCreditId", "dbo.Credits");
            DropForeignKey("dbo.AlertProducts", "CategoryId", "dbo.Categories");
            DropForeignKey("dbo.CategoryPurchaseTypes", "PurchaseTypeId", "dbo.PurchaseTypes");
            DropForeignKey("dbo.PurchaseConfigs", "PackageBaseConfigId", "dbo.PackageBaseConfigs");
            DropForeignKey("dbo.PackageBaseConfigValues", "PackageBaseConfigId", "dbo.PackageBaseConfigs");
            DropForeignKey("dbo.PurchaseConfigs", "CategoryPurchasePackageTypeId", "dbo.CategoryPurchasePackageTypes");
            DropForeignKey("dbo.CategoryPurchasePackageTypes", "PackageTypeId", "dbo.PackageTypes");
            DropForeignKey("dbo.CategoryPurchasePackageTypes", "CategoryPurchaseTypeId", "dbo.CategoryPurchaseTypes");
            DropForeignKey("dbo.CategoryPurchaseTypes", "CategoryId", "dbo.Categories");
            DropForeignKey("dbo.CategoryFeatureDependencies", "CategoryFeature_Id2", "dbo.CategoryFeatures");
            DropForeignKey("dbo.CategoryFeatureDependencies", "CategoryFeature_Id1", "dbo.CategoryFeatures");
            DropForeignKey("dbo.FeatureValueDependencies", "FeatureValue_Id3", "dbo.FeatureValues");
            DropForeignKey("dbo.FeatureValueDependencies", "FeatureValue_Id2", "dbo.FeatureValues");
            DropForeignKey("dbo.FeatureValueDependencies", "FeatureValue_Id1", "dbo.FeatureValues");
            DropForeignKey("dbo.FeatureValueDependencies", "FeatureValue_Id", "dbo.FeatureValues");
            DropForeignKey("dbo.FeatureValueDependencies", "FeatureValueParentId", "dbo.FeatureValues");
            DropForeignKey("dbo.FeatureValueDependencies", "FeatureValueChildId", "dbo.FeatureValues");
            DropForeignKey("dbo.FeatureValueDependencies", "FeatureValueId", "dbo.FeatureValues");
            DropForeignKey("dbo.FeatureValueDependencies", "CategoryFeatureShowId", "dbo.CategoryFeatures");
            DropForeignKey("dbo.FeatureValueDependencies", "CategoryFeatureHideId", "dbo.CategoryFeatures");
            DropForeignKey("dbo.FeatureValues", "CategoryFeatureId", "dbo.CategoryFeatures");
            DropForeignKey("dbo.CategoryFeatures", "ElementId", "dbo.Elements");
            DropForeignKey("dbo.CategoryFeatureDependencies", "CategoryFeature_Id", "dbo.CategoryFeatures");
            DropForeignKey("dbo.CategoryFeatureDependencies", "CategoryFeatureShowId", "dbo.CategoryFeatures");
            DropForeignKey("dbo.CategoryFeatureDependencies", "CategoryFeatureParentId", "dbo.CategoryFeatures");
            DropForeignKey("dbo.CategoryFeatureDependencies", "CategoryFeatureHideId", "dbo.CategoryFeatures");
            DropForeignKey("dbo.CategoryFeatureDependencies", "CategoryFeatureChildId", "dbo.CategoryFeatures");
            DropForeignKey("dbo.CategoryFeatureDependencies", "CategoryFeatureId", "dbo.CategoryFeatures");
            DropForeignKey("dbo.CategoryFeatures", "CategoryId", "dbo.Categories");
            DropForeignKey("dbo.CategoryFeatures", "AndroidElementId", "dbo.AndroidElements");
            DropForeignKey("dbo.CategoryExchanges", "Category_Id", "dbo.Categories");
            DropForeignKey("dbo.AlertFeatureValues", "AlertProductId", "dbo.AlertProducts");
            DropIndex("dbo.VoucherConfigs", new[] { "UserId" });
            DropIndex("dbo.Vouchers", new[] { "VoucherConfigId" });
            DropIndex("dbo.Vouchers", new[] { "CreditId" });
            DropIndex("dbo.Vouchers", new[] { "Code" });
            DropIndex("dbo.TodayPriceConfigs", new[] { "CategoryId" });
            DropIndex("dbo.TodayPriceConfigs", new[] { "UserId" });
            DropIndex("dbo.TodayPriceOptions", new[] { "CategoryId" });
            DropIndex("dbo.Reviews", new[] { "CategoryId" });
            DropIndex("dbo.Reviews", new[] { "UserId" });
            DropIndex("dbo.FeedbackProducts", new[] { "ProductId" });
            DropIndex("dbo.FeedbackProducts", new[] { "UserId" });
            DropIndex("dbo.FeatureValueAssignFeatureValueItems", new[] { "FeatureValueId" });
            DropIndex("dbo.FeatureValueAssignFeatureValueItems", new[] { "FeatureValueAssignId" });
            DropIndex("dbo.FeatureValueAssigns", new[] { "CategoryFeatureId" });
            DropIndex("dbo.FavouriteProducts", new[] { "ProductId" });
            DropIndex("dbo.FavouriteProducts", new[] { "UserId" });
            DropIndex("dbo.ExceptionCodes", new[] { "ExceptionType" });
            DropIndex("dbo.ArticleStructures", new[] { "ParentArticleStructureId" });
            DropIndex("dbo.ArticleStructures", new[] { "CategoryId" });
            DropIndex("dbo.Articles", new[] { "ArticleStructureId" });
            DropIndex("dbo.Articles", new[] { "UserId" });
            DropIndex("dbo.AlertFeatureValueFeatureValueItems", new[] { "FeatureValueId" });
            DropIndex("dbo.AlertFeatureValueFeatureValueItems", new[] { "AlertFeatureValueId" });
            DropIndex("dbo.UserTokens", new[] { "UserId" });
            DropIndex("dbo.Permissions", new[] { "ResourceId" });
            DropIndex("dbo.Permissions", new[] { "RoleId" });
            DropIndex("dbo.UserRoles", new[] { "RoleId" });
            DropIndex("dbo.UserRoles", new[] { "UserId" });
            DropIndex("dbo.ProductFeatureValueFeatureValueItems", new[] { "FeatureValueId" });
            DropIndex("dbo.ProductFeatureValueFeatureValueItems", new[] { "ProductFeatureValueId" });
            DropIndex("dbo.ProductFeatureValues", new[] { "CategoryFeatureId" });
            DropIndex("dbo.ProductFeatureValues", new[] { "ProductId" });
            DropIndex("dbo.ExchangeFeatureValues", new[] { "Product_Id" });
            DropIndex("dbo.ExchangeFeatureValues", new[] { "FeatureValueId" });
            DropIndex("dbo.ExchangeFeatureValues", new[] { "CategoryFeatureId" });
            DropIndex("dbo.DealershipCategories", new[] { "CategoryId" });
            DropIndex("dbo.DealershipCategories", new[] { "DealershipId" });
            DropIndex("dbo.Cities", new[] { "StateId" });
            DropIndex("dbo.Dealerships", new[] { "CityId" });
            DropIndex("dbo.Dealerships", new[] { "UserId" });
            DropIndex("dbo.Products", new[] { "UserPackageCreditId" });
            DropIndex("dbo.Products", new[] { "CategoryId" });
            DropIndex("dbo.Products", new[] { "DealershipId" });
            DropIndex("dbo.Products", new[] { "UserId" });
            DropIndex("dbo.UserPackageCredits", new[] { "CategoryPurchasePackageTypeId" });
            DropIndex("dbo.UserPackageCredits", new[] { "UserId" });
            DropIndex("dbo.Transactions", new[] { "UserId" });
            DropIndex("dbo.Credits", new[] { "UserPackageCreditId" });
            DropIndex("dbo.Credits", new[] { "ParentCreditId" });
            DropIndex("dbo.Credits", new[] { "TransactionId" });
            DropIndex("dbo.Credits", new[] { "UserId" });
            DropIndex("dbo.PackageBaseConfigValues", new[] { "PackageBaseConfigId" });
            DropIndex("dbo.PurchaseConfigs", new[] { "CategoryPurchasePackageTypeId" });
            DropIndex("dbo.PurchaseConfigs", new[] { "PackageBaseConfigId" });
            DropIndex("dbo.CategoryPurchasePackageTypes", new[] { "PackageTypeId" });
            DropIndex("dbo.CategoryPurchasePackageTypes", new[] { "CategoryPurchaseTypeId" });
            DropIndex("dbo.CategoryPurchaseTypes", new[] { "PurchaseTypeId" });
            DropIndex("dbo.CategoryPurchaseTypes", new[] { "CategoryId" });
            DropIndex("dbo.FeatureValueDependencies", new[] { "FeatureValue_Id3" });
            DropIndex("dbo.FeatureValueDependencies", new[] { "FeatureValue_Id2" });
            DropIndex("dbo.FeatureValueDependencies", new[] { "FeatureValue_Id1" });
            DropIndex("dbo.FeatureValueDependencies", new[] { "FeatureValue_Id" });
            DropIndex("dbo.FeatureValueDependencies", new[] { "CategoryFeatureShowId" });
            DropIndex("dbo.FeatureValueDependencies", new[] { "CategoryFeatureHideId" });
            DropIndex("dbo.FeatureValueDependencies", new[] { "FeatureValueChildId" });
            DropIndex("dbo.FeatureValueDependencies", new[] { "FeatureValueParentId" });
            DropIndex("dbo.FeatureValueDependencies", new[] { "FeatureValueId" });
            DropIndex("dbo.FeatureValues", new[] { "CategoryFeatureId" });
            DropIndex("dbo.CategoryFeatureDependencies", new[] { "CategoryFeature_Id2" });
            DropIndex("dbo.CategoryFeatureDependencies", new[] { "CategoryFeature_Id1" });
            DropIndex("dbo.CategoryFeatureDependencies", new[] { "CategoryFeature_Id" });
            DropIndex("dbo.CategoryFeatureDependencies", new[] { "CategoryFeatureShowId" });
            DropIndex("dbo.CategoryFeatureDependencies", new[] { "CategoryFeatureHideId" });
            DropIndex("dbo.CategoryFeatureDependencies", new[] { "CategoryFeatureChildId" });
            DropIndex("dbo.CategoryFeatureDependencies", new[] { "CategoryFeatureParentId" });
            DropIndex("dbo.CategoryFeatureDependencies", new[] { "CategoryFeatureId" });
            DropIndex("dbo.CategoryFeatures", new[] { "AndroidElementId" });
            DropIndex("dbo.CategoryFeatures", new[] { "ElementId" });
            DropIndex("dbo.CategoryFeatures", new[] { "CategoryId" });
            DropIndex("dbo.CategoryExchanges", new[] { "Category_Id" });
            DropIndex("dbo.AlertProducts", new[] { "CategoryId" });
            DropIndex("dbo.AlertProducts", new[] { "UserId" });
            DropIndex("dbo.AlertFeatureValues", new[] { "CategoryFeatureId" });
            DropIndex("dbo.AlertFeatureValues", new[] { "AlertProductId" });
            DropTable("dbo.VoucherConfigs");
            DropTable("dbo.Vouchers");
            DropTable("dbo.TodayPriceConfigs");
            DropTable("dbo.TodayPriceOptions");
            DropTable("dbo.SMS");
            DropTable("dbo.Reviews");
            DropTable("dbo.RecabSystemConfigs");
            DropTable("dbo.MongoBackUps");
            DropTable("dbo.Media");
            DropTable("dbo.FeedbackProducts");
            DropTable("dbo.FeatureValueAssignFeatureValueItems");
            DropTable("dbo.FeatureValueAssigns");
            DropTable("dbo.FavouriteProducts");
            DropTable("dbo.ExceptionCodes");
            DropTable("dbo.ArticleStructures");
            DropTable("dbo.Articles");
            DropTable("dbo.AlertFeatureValueFeatureValueItems");
            DropTable("dbo.UserTokens");
            DropTable("dbo.Resources");
            DropTable("dbo.Permissions");
            DropTable("dbo.Roles");
            DropTable("dbo.UserRoles");
            DropTable("dbo.ProductFeatureValueFeatureValueItems");
            DropTable("dbo.ProductFeatureValues");
            DropTable("dbo.ExchangeFeatureValues");
            DropTable("dbo.DealershipCategories");
            DropTable("dbo.States");
            DropTable("dbo.Cities");
            DropTable("dbo.Dealerships");
            DropTable("dbo.Products");
            DropTable("dbo.UserPackageCredits");
            DropTable("dbo.Transactions");
            DropTable("dbo.Credits");
            DropTable("dbo.Users");
            DropTable("dbo.PurchaseTypes");
            DropTable("dbo.PackageBaseConfigValues");
            DropTable("dbo.PackageBaseConfigs");
            DropTable("dbo.PurchaseConfigs");
            DropTable("dbo.PackageTypes");
            DropTable("dbo.CategoryPurchasePackageTypes");
            DropTable("dbo.CategoryPurchaseTypes");
            DropTable("dbo.FeatureValueDependencies");
            DropTable("dbo.FeatureValues");
            DropTable("dbo.Elements");
            DropTable("dbo.CategoryFeatureDependencies");
            DropTable("dbo.AndroidElements");
            DropTable("dbo.CategoryFeatures");
            DropTable("dbo.CategoryExchanges");
            DropTable("dbo.Categories");
            DropTable("dbo.AlertProducts");
            DropTable("dbo.AlertFeatureValues");
        }
    }
}
