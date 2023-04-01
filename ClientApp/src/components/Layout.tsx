import * as React from 'react';
import {Container} from 'reactstrap'
import {Header} from './header';
import {Home} from './Home';

export default class Layout extends React.PureComponent<{}, { children?: React.ReactNode }> {
    public render() {
        return (
            <React.Fragment>
                <Header />
             
                <Container>
              
                    {this.props.children}
                </Container>
            </React.Fragment>
        );
    }
}