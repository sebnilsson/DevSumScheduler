const path = require('path');
const VueLoaderPlugin = require('vue-loader/lib/plugin');

const distDir = path.resolve(__dirname, './wwwroot/dist/');
const nodeModulesDir = path.resolve(__dirname, './node_modules');

module.exports = {
    entry: { 'vue-app': ['./src/main.ts'] },
    output: {
        path: distDir,
        filename: 'app.js'
    },
    resolve: {
        extensions: ['.ts', '.js', '.html'],
        //modules: [srcDir, sharedDir, 'node_modules'],
        alias: {
            'vue$': 'vue/dist/vue.esm.js'
        }
    },
    module: {
        rules: [
            {
                test: /\.tsx?$/i,
                loader: 'ts-loader',
                exclude: nodeModulesDir,
                options: {
                    appendTsSuffixTo: [/\.vue$/]
                }
            },
            {
                test: /\.vue$/i,
                loader: 'vue-loader'
            }
        ]
    },
    plugins: [
        new VueLoaderPlugin()
    ]
};