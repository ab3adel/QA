import * as React from 'react';
import { Route , Switch} from 'react-router';
import Layout from './components/Layout';
import {SearchPage} from './components/searchpage'
import {SignInPage} from './auth/signinpage'
import {SignOut} from './auth/signout'
import {QuestionPage} from './components/questionpage'
import {AskPage} from './components/askpage'
const NotFoundPage= React.lazy(()=>import ('./components/notfoundpage'))
import {Home} from './components/Home';



export default () => (
    <Layout>
    <Switch>
        <Route exact path="/questionpage/:questionId" component={QuestionPage} />
        <Route exact path='/search' component={SearchPage} />
        <Route exact path="/signin">
            <SignInPage action="signin"/>
        </Route>
        <Route exact path="/signin-callback">
             <SignInPage action="signin-callback"/>
        </Route>
        <Route  exact path="/signout">
            <SignOut action ="signout"/>
        </Route>
        <Route exact path="/signout-callback">
            <SignOut action ="signout-callback"/>
        </Route>
       <Route exact path="/ask" component={AskPage}/>
        <Route exact path="/" component ={Home}/>
        <React.Suspense fallback={<div>...loading</div>}><Route  path='*' component={NotFoundPage} /></React.Suspense>
    </Switch>    
    </Layout>
);
