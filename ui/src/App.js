import React from 'react';
import './App.css';
import './index.css';
import { notification } from 'antd'
import AppComponent from './components/AppComponent/AppComponent';

notification.config({
  placement: 'topRight',
  duration: 10
});

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