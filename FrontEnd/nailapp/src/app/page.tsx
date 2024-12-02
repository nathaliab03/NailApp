"use client"
import React, { useState, useEffect } from "react";
import Link from "next/link";
import { EnvelopeIcon, LockClosedIcon } from '@heroicons/react/24/outline';
import { useRouter } from "next/navigation";

export default function Home() {

  const router = useRouter();
  const [email, setEmail] = useState<string>('');
  const [password, setPassword] = useState<string>('');
  const [error, setError] = useState<string | null>('');

  const authentication = async (e: any) => {
    e.preventDefault();
    setError(null);

    if (email != "" && password != "") {

      const formData = {
        Email: email,
        Password: password,
        RememberMe: false
      }

      const add = await fetch(`${process.env.NEXT_PUBLIC_API_BASE_URL}/api/account/login`, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(formData),
      });

      const content = await add.json();
      if (content.token) {
        sessionStorage.setItem("token", content.token);
        sessionStorage.setItem("role", content.role);
        router.push('/home');
      } else {
        setError(content.error);
      }

    }
  }

  return (
    
    <div className="flex items-center justify-center h-screen">
      <form className='continer w-96' onSubmit={authentication}>
        <span className='font-bold text-white py-2 block text-2xl text-center'>Entrar</span>
        <div className='relative w-full py-2'>
        <EnvelopeIcon className="absolute left-3 top-1/2 transform -translate-y-1/2 h-6 w-6 text-gray-400" />
          <input type='text' name='name' className='w-full input-custom pl-11 pt-3.5 rounded-sm focus:outline-none' placeholder="Email ou Username" onChange={(e: any) => setEmail(e.target.value)} />
        </div>
        <div className='relative w-full py-2'>
        <LockClosedIcon className="absolute left-3 top-1/2 transform -translate-y-1/2 h-6 w-6 text-gray-400" />
          <input name='email' type="password" className='w-full input-custom pl-11 pt-3.5 rounded-sm focus:outline-none' placeholder="Senha" onChange={(e: any) => setPassword(e.target.value)} />
        </div>
        <div className='w-full py-2'>
          <button className="w-full p-2 border-gray-200 border-[1px] rounded-sm btn-primary uppercase">Entrar</button>
        </div>
        <div>
          {error && <div className="p-2 text-white border-gray-200 border-[1px] rounded-sm bg-red-400" style={{ color: 'red' }}>{error}</div>}
        </div>
      </form>
      <div className="absolute bottom-14 text-center">
        <p className="text-white">
          Não tem conta? <br />
          <Link href="/signup/create">Cadastre-se</Link>
        </p>
      </div>
    </div>
  )
}