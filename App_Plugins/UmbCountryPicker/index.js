angular.module("umbraco").controller("UmbCountryPickerController", function($scope, $http, $routeParams, userService) {
   $scope.model.value = $scope.model.value
   $scope.model.userLocale = ""
   $scope.model.countries = [];
   $scope.model.showCountries = false
   // Default value
   $scope.userCulture = "en-US"
   
   $scope.model.showCountriesList = function (){
      $scope.model.showCountries = !$scope.model.showCountries;
   }
   
   // Close dropdown when a country is selected.
   $scope.model.onCountrySelect = function() {
      $scope.model.showCountries = false;
   }
   
   // Use this to close dropdown by pressing the escape key
   $scope.model.countryKeyDown = function(event) {
      if (event.key === "Escape" || event.keyCode === 27) {
         $scope.model.showCountries = false;
      }
   }

   $scope.model.getSelectedCountryName = function () {
      const selected = $scope.model.countries.find(c => c.Id === $scope.model.value);
      return selected ? selected.CountryName : '';
   }


   function loadCountries () {
      $http.get(`/umbraco/backoffice/UmbCountryPicker/UmbCountryPickerApi/GetKeyValueList?languageIsoCodeString=${$scope.model.userLocale}`).then(function(response) {
         // the api returns multiple arrays. Concat them into 1 to make orderBy work
         $scope.model.countries = $scope.model.countries.concat(response.data);

         // Sort the full list every time
         $scope.model.countries.sort((a, b) => a.CountryName.localeCompare(b.CountryName));
      }).catch(err => {
         console.log("Error when fetching countries")
         console.log(err)
      })
   }

   function init() {
      userService.getCurrentUser().then(function (user) {
         $scope.model.userLocale = user.locale;
         loadCountries()
         console.log($scope.model.value)
      })
   }

   init();

});