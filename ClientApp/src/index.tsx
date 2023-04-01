import 'bootstrap/dist/css/bootstrap.css';
import {Provider} from 'react-redux'
import {configureStore} from './components/store'
import * as ReactDOM from 'react-dom';
import {BrowserRouter} from 'react-router-dom'
import App from './App';
import {AuthProvider} from './auth/Auth'
const store =configureStore()
ReactDOM.render(
    <Provider store={store}>
    <AuthProvider>
    <BrowserRouter>
 
     <App />
   
    </BrowserRouter>  
    </AuthProvider> 
    </Provider>   
,
    document.getElementById('root'));


