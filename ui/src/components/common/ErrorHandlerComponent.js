import React from 'react';

class ErrorHandlerComponent extends React.Component {
    constructor(props) {
        super(props);
        this.state = { hasError: false };
    }

    componentDidCatch(error, info){
        console.log(error);
        console.log(info);
        console.log(error.response);
        this.setState({hasError: true});
    }

    render(){
        if (this.state.hasError) {
            // You can render any custom fallback UI
            return <h1>Something went wrong.</h1>;
          }
          return this.props.children;
    }
}

export default ErrorHandlerComponent;