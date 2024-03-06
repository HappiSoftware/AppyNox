# Changelog
All notable changes to AppyNox will be documented in this file.

## [1.6.0](https://github.com/HappiSoftware/AppyNox/compare/v1.5.0...v1.6.0) - NOT RELEASED
### Added
- Feature request: Create methods should return id only ([#237](https://github.com/HappiSoftware/AppyNox/issues/237))


## [1.5.0](https://github.com/HappiSoftware/AppyNox/compare/v1.4.0...v1.5.0) - 2024.02.29
### Added
- Feature request: Superadmin role bypass authorization ([#229](https://github.com/HappiSoftware/AppyNox/issues/229))
- Feature request: Coupon Service should has Anemic Domain Modeling Example ([#223](https://github.com/HappiSoftware/AppyNox/issues/223))

### Fixed
- Bug report: NoxRepository GetById should use projection ([#226](https://github.com/HappiSoftware/AppyNox/issues/226))
- Refactoring request: Generic Repository and CQRS Refactor ([#222](https://github.com/HappiSoftware/AppyNox/issues/222))


## [1.4.0](https://github.com/HappiSoftware/AppyNox/compare/v1.3.0...v1.4.0) - 2024.02.26
### Added
- Feature request: NoxRepository and Cqrs Methods ([#201](https://github.com/HappiSoftware/AppyNox/issues/201))
- Feature request: CouponService Should use DDD ([#209](https://github.com/HappiSoftware/AppyNox/issues/209))

### Fixed
- Bug Report: Generic Repository Update Method Throws Error ([#199](https://github.com/HappiSoftware/AppyNox/issues/199))


## [1.3.0](https://github.com/HappiSoftware/AppyNox/compare/v1.2.0...v1.3.0) - 2024.02.16
### Fixed
- NoxJwtAuthenticationHandler Should Validate Token Time ([#179](https://github.com/HappiSoftware/AppyNox/issues/179))
- Failed C.I. Runs ([#186](https://github.com/HappiSoftware/AppyNox/issues/186))
- Bug Report: Generic CQRS/Repository Methods Incompatibility with DDD Classes and Deep Navigation Property Fetching Issues ([#195](https://github.com/HappiSoftware/AppyNox/issues/195))
- Bug Report: NoxApiResponse Not Setting Response Code ([#192](https://github.com/HappiSoftware/AppyNox/issues/192))

### Added
- Pagination Data Should be Returned in Response ([#173](https://github.com/HappiSoftware/AppyNox/issues/173))
- SSO Service Integration Tests ([#78](https://github.com/HappiSoftware/AppyNox/issues/78))


## [1.2.1](https://github.com/HappiSoftware/AppyNox/compare/v1.2.0...v1.2.1) - 2024.02.15
### Added
- Pagination Data Should be Returned in Response ([#173](https://github.com/HappiSoftware/AppyNox/issues/173))

### Fixed
- Failed C.I. Runs ([#186](https://github.com/HappiSoftware/AppyNox/issues/186))


## [1.2.0](https://github.com/HappiSoftware/AppyNox/compare/v1.1.5...v1.2.0) - 2024.02.12
### Added
- Renaming Authentication Service ([#169](https://github.com/HappiSoftware/AppyNox/issues/169))
- EntityBase Should not contain Code ([#176](https://github.com/HappiSoftware/AppyNox/issues/176))


## [1.1.5](https://github.com/HappiSoftware/AppyNox/compare/v1.1.4...v1.1.5) - 2024.02.08
### Added
- NoxContextMiddleware Should Allow Swagger ([#161](https://github.com/HappiSoftware/AppyNox/issues/161))
- NoxResponseWrapperMiddleware WriteResponseAsync should be able to handle Exception in Body ([#163](https://github.com/HappiSoftware/AppyNox/issues/163))
- GenericRepositoryBase Get Methods Should Have AsNoTracking ([#165](https://github.com/HappiSoftware/AppyNox/issues/165))


## [1.1.4](https://github.com/HappiSoftware/AppyNox/compare/v1.1.3...v1.1.4) - 2024.02.07
### Added
- Nox Should Support DDD ([#159](https://github.com/HappiSoftware/AppyNox/issues/159))


## [1.1.3](https://github.com/HappiSoftware/AppyNox/compare/v1.1.2...v1.1.3) - 2024.02.05
### Added
- PackageRequireLicenseAcceptance added to Nox Packages

### Changed
- Symbols removed from nuget packages


## [1.1.2](https://github.com/HappiSoftware/AppyNox/compare/v1.1.1...v1.1.2) - 2024.02.05
### Added
- Symbols added to nuget packages


## [1.1.1](https://github.com/HappiSoftware/AppyNox/compare/v1.1.0...v1.1.1) - 2024.02.05
### Added
- Licenses added to nuget packages


## [1.1.0](https://github.com/HappiSoftware/AppyNox/compare/v1.0.5...v1.1.0) - 2024.02.04
### Changed
- UserId Context comes Null in Wrapper Middleware ([#153](https://github.com/HappiSoftware/AppyNox/issues/153))
- NoxLocalizations Will Be Marked as Internal ([#154](https://github.com/HappiSoftware/AppyNox/issues/154))


## [1.0.5](https://github.com/HappiSoftware/AppyNox/compare/v1.0.4...v1.0.5) - 2024.01.29
### Added
- Localization ([#146](https://github.com/HappiSoftware/AppyNox/issues/146))
- Adding Exception Code to NoxException ([#144](https://github.com/HappiSoftware/AppyNox/issues/144))


## [1.0.4](https://github.com/HappiSoftware/AppyNox/compare/v1.0.3...v1.0.4) - 2024.01.22
### Added
- Adding CORS to Gateway ([#133](https://github.com/HappiSoftware/AppyNox/issues/133))
- ApiResponse Should Be Removed From Endpoints ([#135](https://github.com/HappiSoftware/AppyNox/issues/135))
- NoxJwtAuthenticationHandler / NoxJwtAuthorizationHandler ([#136](https://github.com/HappiSoftware/AppyNox/issues/136))

### Changed
- EntityBase Improvement ([#138](https://github.com/HappiSoftware/AppyNox/issues/138))


## [1.0.3](https://github.com/HappiSoftware/AppyNox/compare/v1.0.2...v1.0.3) - 2024.01.19
### Added
- AppyFleet Requests For Base WebAPI ([#123](https://github.com/HappiSoftware/AppyNox/issues/123))
- Adding Product to LicenseService ([#128](https://github.com/HappiSoftware/AppyNox/issues/128))
- AuthenticationService Improvements ([#131](https://github.com/HappiSoftware/AppyNox/issues/131))

### Maintenance
- AppyNox-127 AuthenticationService Refactor ([#127](https://github.com/HappiSoftware/AppyNox/issues/127))


## [1.0.2](https://github.com/HappiSoftware/AppyNox/compare/v1.0.1...v1.0.2) - 2024.01.15
### Maintenance
- Weekly Sonar Cleanup (First week of January 2024) ([#121](https://github.com/HappiSoftware/AppyNox/issues/121))


## [1.0.1](https://github.com/HappiSoftware/AppyNox/compare/v1.0.0...v1.0.1) - 2024.01.15
### Added
- Added Auditable data support. ([#112](https://github.com/HappiSoftware/AppyNox/issues/112))

### Changed
- Auditable data support improved. ([#116](https://github.com/HappiSoftware/AppyNox/issues/116))


## [1.0.0](https://github.com/HappiSoftware/AppyNox/releases/tag/v1.0.0) - 2024-01-11
- Initial release
