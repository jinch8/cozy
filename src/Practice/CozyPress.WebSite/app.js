﻿define([
    'app.routeConfig',
    'app.loader',
    'angular',
    'angular-resource',
    'angular-route'
], function (config, loader) {
    'use strict';

    var app = angular.module('app', ['ngRoute', 'ngLocale', 'ngResource']);
    app.config(configure);

    configure.$inject = ['$routeProvider', '$locationProvider', '$controllerProvider', '$compileProvider', '$filterProvider', '$provide'];

    return app;

    function configure($routeProvider, $locationProvider, $controllerProvider, $compileProvider, $filterProvider, $provide) {
        app.registerController = $controllerProvider.register;
        app.registerDirective = $compileProvider.directive;
        app.registerFilter = $filterProvider.register;
        app.registerFactory = $provide.factory;
        app.registerService = $provide.service;

        if (config.routes != undefined) {
            angular.forEach(config.routes, function (route, path) {
                $routeProvider.when(path, {
                    templateUrl: route.templateUrl,
                    controller: route.controller,
                    resolve: loader(route.dependencies)
                });
            });
        }

        if (config.defaultRoute != undefined) {
            $routeProvider.otherwise({ redirectTo: config.defaultRoute });
        }
    }
});