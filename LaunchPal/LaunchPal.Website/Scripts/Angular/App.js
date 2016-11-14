// Register app in Angular
var app = angular.module('myApp', ["ngRoute"]);

// Setting up routing in the app, adapted to MVC Controller behavior
app.config(['$routeProvider', '$locationProvider', function ($routeProvider, $locationProvider, $http) {

    $routeProvider.when('/', {
        templateUrl: '/AngularView/Index',
        controller: 'LaunchCtrl'
    });
    $routeProvider.when('/Home/', {
        templateUrl: '/AngularView/Index',
        controller: 'LaunchCtrl'
    });
    $routeProvider.when('/Privacy', {
        templateUrl: '/AngularView/Privacy',
        controller: 'LaunchCtrl'
    });
    $routeProvider.when('/Home/Privacy', {
        templateUrl: '/AngularView/Privacy',
        controller: 'LaunchCtrl'
    });
    $routeProvider.otherwise({
        redirectTo: '/'
    });

    // Specify HTML5 mode (using the History APIs) or HashBang syntax.
    //$locationProvider.html5Mode({ enabled: true, requireBase: false });
    $locationProvider.html5Mode(false);

}]);