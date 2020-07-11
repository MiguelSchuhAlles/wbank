import React, { Component } from 'react';
import './App.css';

import FullWidthTabs from './Components/FullWidthTabs';

const userData = {
  Email: "diogo@gmail.com",
  Password: "diogo"
};

class App extends Component {
  state = {
    token: "",
    userId: 0,
    accountId: 0
  }

  componentDidMount() {
    fetch('https://localhost:44337/api/authentication',{
      method: "POST",
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify(userData),
    })
    .then(res => res.json())
    .then((data) => {
      this.setState({ token: data.token, userId: data.userId, accountId: data.accountId });
    })
    .catch(console.log)
  }

  render() {
    if(this.state.token){
      return <div className="App">{}
          <FullWidthTabs token={this.state.token} accountId={this.state.accountId}></FullWidthTabs>
      </div>
    }
    else return <div></div>
  };
}

export default App;
