export default [
  {
    path: '/',
    component: () => import('layouts/default'),
    children: [
      { path: '', component: () => import('pages/CoreLabs') },
      { path: '/', component: () => import('pages/CoreLabs') },
      { path: '/corelabs', component: () => import('pages/CoreLabs') },
      { path: '/presets', component: () => import('pages/Presets') },
      { path: '/alpha', component: () => import('pages/Alpha') },
      { path: '/audio', component: () => import('pages/Audio') },
      { path: '/history', component: () => import('pages/History') },
      { path: '/video', component: () => import('pages/Video') }
    ]
  },
  { // Always leave this as last one
    path: '*',
    component: () => import('pages/404')
  }
]
