"use client";
import Link from "next/link";
import React, { useState, useEffect } from "react";

interface Agenda {
  date: string;
  hours: string[];
}

interface NailStylist {
  id: number;
  name: string;
}

export default function Home() {
  const [nailStylists, setNailStylists] = useState<NailStylist[]>([]);
  const [selectedStylist, setSelectedStylist] = useState<number | null>(null);
  const [agenda, setAgenda] = useState<Agenda[]>([]);

  useEffect(() => {
    // Fetch lista de NailStylists
    fetch(`${process.env.NEXT_PUBLIC_API_BASE_URL}/api/nailstylist`, {
      headers: {
        "Content-Type": "application/json",
        Authorization: sessionStorage.getItem("token") || "",
      },
    })
      .then((response) => response.json())
      .then((data) => setNailStylists(data.content));
  }, []);

  const fetchAgenda = (id: number) => {
    // Fetch agenda da NailStylist selecionada
    fetch(`${process.env.NEXT_PUBLIC_API_BASE_URL}/api/appointment/availableDates?stylistId=${id}`, {
      headers: {
        "Content-Type": "application/json",
        Authorization: sessionStorage.getItem("token") || "",
      },
    })
      .then((response) => response.json())
      .then((data) => setAgenda(data.content))
      .catch(() => setAgenda([]));
  };

  return (
    <div className="flex items-center justify-center min-h-screen">
        <div className="p-6 rounded-lg shadow-lg w-full max-w-2xl">

        <div className="w-full flex justify-center py-3 mt-5">
          <Link href="/appointments/create" className="w-full p-2 text-center border-gray-200 border-[1px] rounded-sm btn-primary uppercase">Agendar horário</Link>
        </div>
      <h1 className="text-3xl font-bold mb-5">Agenda das NailStylists</h1>

      {/* Filtro de NailStylist */}
      <div className="mb-5">
        <label htmlFor="stylistSelect" className="text-2xl font-bold text-center mb-6">
          Selecione uma NailStylist:
        </label>
        <select
          id="stylistSelect"
          className="mt-1 input-custom block w-full border-gray-300 rounded-md shadow-sm focus:ring-indigo-500 focus:border-indigo-500"
          value={selectedStylist || ""}
          onChange={(e) => {
            const stylistId = parseInt(e.target.value, 10);
            setSelectedStylist(stylistId);
            fetchAgenda(stylistId);
          }}
        >
          <option value="">Selecione</option>
          {nailStylists.map((stylist) => (
            <option key={stylist.id} value={stylist.id}>
              {stylist.name}
            </option>
          ))}
        </select>
      </div>

      {/* Lista de Agenda */}
      {agenda.length > 0 ? (
        <ul className="border-t border-gray-200 divide-y divide-gray-200">
          {agenda.map((day) => (
            <li key={day.date} className="py-4 flex justify-between">
              <span className="font-medium">{new Date(day.date).toLocaleDateString()}</span>
              <span>{day.hours.join(", ")}</span>
            </li>
          ))}
        </ul>
      ) : (
        <p className="text-white-500">Selecione uma NailStylist para visualizar a agenda.</p>
      )}
      </div>
    </div>
  );
}
