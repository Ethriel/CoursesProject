import React from 'react';
import './App.css';
import './index.css';
import AppComponent from './components/AppComponent/AppComponent';

class App extends React.Component {
  render() {
    return (
      <div className="App">
        <AppComponent />
      </div>
    );
  }
};

export default App;