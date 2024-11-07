// Configuration for your app

module.exports = function (ctx) {
  return {
    // app plugins (/src/plugins)
    plugins: [
      'axios',
      'fontawesome-pro'
    ],
    css: [
      'app.styl'
    ],
    extras: [
      ctx.theme.mat ? 'roboto-font' : null,
      'material-icons'
      // 'ionicons',
      // 'mdi',
      // 'fontawesome'
    ],
    supportIE: false,
    vendor: {
      add: [],
      remove: []
    },
    build: {
      // env: ctx.dev ? {
      //   __ENV__: JSON.stringify("Development"),
      //   __API__: JSON.stringify("http://localhost:1111/Zone/Api"),
      //   __STORE__: JSON.stringify("http://localhost:1111/Zone/Api/Config")
      // } : {
      //   __ENV__: JSON.stringify('Production'),
      //   __API__: JSON.stringify('/Zone/Api'),
      //   __STORE__: JSON.stringify('/Zone/Api/Config')
      // },
      env: {
      // Dev
      //        __ENV__: JSON.stringify("Development"),
      //        __API__: JSON.stringify("http://localhost:1111/Zone/Api"),
      //        __STORE__: JSON.stringify("http://localhost:1111/Zone/Api/Config")

      // Prod
        __ENV__: JSON.stringify('Production'),
        __API__: JSON.stringify('http://localhost:2222/Api'),
        __STORE__: JSON.stringify('http://localhost:2222/Api/Config')
      },
      scopeHoisting: true,
      vueRouterMode: 'hash',
      publicPath: '/',
      distDir: '../wwwroot',
      // gzip: true,
      // analyze: true,
      // extractCSS: false,
      // useNotifier: false,
      extendWebpack (cfg) {
        cfg.module.rules.push({
          enforce: 'pre',
          test: /\.(js|vue)$/,
          loader: 'eslint-loader',
          exclude: /(node_modules|quasar)/
        })
      }
    },
    devServer: {
      // https: true,
      // port: 8080,
      open: true // opens browser window automatically
    },
    // framework: 'all' --- includes everything; for dev only!
    framework: {
        components: [
        'QLayout',
        'QLayoutHeader',
        'QLayoutFooter',
        'QLayoutDrawer',
        'QPageContainer',
        'QPage',
        'QToolbar',
        'QToolbarTitle',
        'QModal',
        'QBtn',
        'QIcon',
        'QList',
        'QListHeader',
        'QItem',
        'QItemMain',
        'QItemTile',
        'QItemSide',
        'QTabs',
        'QRouteTab',
        'QCard',
        'QCardTitle',
        'QCard',
        'QCardTitle',
        'QCardMain',
        'QCardMedia',
        'QCardActions',
        'QCardSeparator',
        'QIcon',
        'Ripple'
      ],
      directives: [
        'Ripple'
      ],
      // Quasar plugins
      plugins: [
      ],
      iconSet: 'fontawesome-pro'
    },
    // animations: 'all' --- includes all animations
    animations: [
    ],
    pwa: {
      cacheExt: 'js,html,css,ttf,eot,otf,woff,woff2,json,svg,gif,jpg,jpeg,png,wav,ogg,webm,flac,aac,mp4,mp3',
      manifest: {
        id: "alpha",
        short_name: "ALPHA",
        scope: ".",
        start_url: "/",
        display: 'standalone',
        orientation: 'portrait',
        background_color: '#000000',
        theme_color: '#027be3',
        description: "Z2 Alpha Controller",
        icons: [
          {
            'src': 'statics/icons/icon-128x128.png',
            'sizes': '128x128',
            'type': 'image/png'
          },
          {
            'src': 'statics/icons/icon-192x192.png',
            'sizes': '192x192',
            'type': 'image/png'
          },
          {
            'src': 'statics/icons/icon-256x256.png',
            'sizes': '256x256',
            'type': 'image/png'
          },
          {
            'src': 'statics/icons/icon-384x384.png',
            'sizes': '384x384',
            'type': 'image/png'
          },
          {
            'src': 'statics/icons/icon-512x512.png',
            'sizes': '512x512',
            'type': 'image/png'
          }
        ]
      }
    },
    cordova: {
      // id: 'org.cordova.quasar.app'
    },
    electron: {
      extendWebpack (cfg) {
      },
      packager: {

      }
    },

    // leave this here for Quasar CLI
    starterKit: '1.0.2'
  }
}
